using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CourseApp.API.Dtos;
using CourseApp.API.Helpers;
using CourseApp.API.IRepositories;
using CourseApp.API.Model;
using Microsoft.AspNetCore.Mvc;

namespace CourseApp.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Route("api/users/{userId}/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IRepositoryWrapper _repo;
        private readonly IMapper _mapper;
        public MessagesController(IRepositoryWrapper repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }

        [HttpGet("{id}", Name = "GetMessage")]
        public async Task<IActionResult> GetMessage(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var messageFromRepo = await _repo.MessageRepository.GetMessageAsync(id);
            if (messageFromRepo == null)
            {
                return NotFound();
            }
            return Ok(messageFromRepo);
        }

        [HttpGet]
        public async Task<IActionResult> GetMessagesForUser(int userId, [FromQuery] MessageParams messageParams)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            messageParams.UserId = userId;
            var messagesFromRepo = await _repo.MessageRepository.GetMessagesForUserAsync(messageParams);
            var messages = _mapper.Map<IEnumerable<MessageToReturnDto>>(messagesFromRepo);
            Response.AddPagination(messagesFromRepo.CurrentPage, messagesFromRepo.PageSize, messagesFromRepo.TotalItems, messagesFromRepo.TotalPages);
            return Ok(messages);

        }
        [HttpGet("thread/{recipientId}")]
        public async Task<IActionResult> GetMessageThread(int userId, int recipientId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var messagesFromRepo = await _repo.MessageRepository.GetMessageThreadAsync(userId, recipientId);
            var messageThread = _mapper.Map<IEnumerable<MessageToReturnDto>>(messagesFromRepo);

            return Ok(messageThread);



        }



        [HttpPost]
        public async Task<IActionResult> CreateMessage(int userId, MessageForCreationDto messageForCreationDto)
        {
            var sender = await _repo.UserRepository.GetUserAsync(userId);

            if (sender.Id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            messageForCreationDto.SenderId = userId;
            var recipient = await _repo.UserRepository.GetUserAsync(messageForCreationDto.RecipientId);
            if (recipient == null)
                return BadRequest("Couldn't find user");

            var message = _mapper.Map<Message>(messageForCreationDto);

            _repo.MessageRepository.Add(message);

            if (await _repo.MessageRepository.SaveAllAsync())
            {
                var messageToReturn = _mapper.Map<MessageToReturnDto>(message);

                return CreatedAtRoute("GetMessage", new { id = message.Id }, messageToReturn);

            }

            throw new Exception("Creating the message failed on save");
        }
        [HttpPost("{id}")]
        public async Task<IActionResult> DeleteMessage(int id, int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var messageFromRepo = await _repo.MessageRepository.GetMessageAsync(id);
            if (messageFromRepo.SenderId == userId)
                messageFromRepo.SenderDeleted = true;
            if (messageFromRepo.RecipientId == userId)
                messageFromRepo.RecipientDeleted = true;

            if (messageFromRepo.SenderDeleted && messageFromRepo.RecipientDeleted)
                _repo.MessageRepository.Delete(messageFromRepo);

            if (await _repo.MessageRepository.SaveAllAsync())
                return NoContent();

            throw new Exception("Error deleting the message");

        }
        [HttpPost("{id}/read")]
        public async Task<IActionResult> MarkMessageAsRead(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            var messageFromRepo = await _repo.MessageRepository.GetMessageAsync(id);

            if (messageFromRepo.RecipientId != userId)
                return Unauthorized();
            messageFromRepo.IsRead = true;
            messageFromRepo.DateRead = DateTime.Now;

            if (await _repo.MessageRepository.SaveAllAsync())
                return NoContent();
            throw new Exception("Error marking message as read");
        }

    }
}