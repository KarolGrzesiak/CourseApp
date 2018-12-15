using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CourseApp.API.Dtos;
using CourseApp.API.Helpers;
using CourseApp.API.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseApp.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IRepositoryWrapper _repo;
        private readonly IMapper _mapper;
        public UsersController(IRepositoryWrapper repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;

        }
        [HttpGet]
        public async Task<IActionResult> GetUsersAsync([FromQuery] UserParams userParams)
        {

            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var userfromRepo = await _repo.UserRepository.GetUserAsync(currentUserId);

            userParams.UserId = currentUserId;

            var users = await _repo.UserRepository.GetUsersAsync(userParams);
            var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);
            Response.AddPagination(users.CurrentPage, users.PageSize, users.TotalItems, users.TotalPages);

            return Ok(usersToReturn);
        }

        [HttpGet("{id}", Name = "GetUserAsync")]
        public async Task<IActionResult> GetUserAsync(int id)
        {

            var user = await _repo.UserRepository.GetUserAsync(id);
            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserForUpdateDto userForUpdateDto)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var userfromRepo = await _repo.UserRepository.GetUserAsync(id);

            _mapper.Map(userForUpdateDto, userfromRepo);
            if (await _repo.UserRepository.SaveAllAsync())
                return NoContent();

            throw new Exception($"Updating user {id} failed on save");
        }

    }
}