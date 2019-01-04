using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CourseApp.API.Dtos;
using CourseApp.API.Helpers;
using CourseApp.API.IRepositories;
using CourseApp.API.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamsController : ControllerBase
    {
        private readonly IRepositoryWrapper _repo;
        private readonly IMapper _mapper;
        public ExamsController(IRepositoryWrapper repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;

        }

        [HttpGet("{examId}", Name = "GetExamAsync")]
        public async Task<IActionResult> GetExamAsync(int examId)
        {
            var examFromRepo = await _repo.ExamRepository.GetExamAsync(examId);
            if (examFromRepo == null)
                return NotFound();
            return Ok(examFromRepo);
        }
        [HttpGet(Name = "GetExamsAsync")]
        public async Task<IActionResult> GetExamsAsync([FromQuery] int? pageNumber, [FromQuery] int? pageSize)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var examsFromRepo = await _repo.ExamRepository.GetExamsAsync(pageNumber, pageSize, userId);
            var examsForReturn = _mapper.Map<IEnumerable<ExamForListDto>>(examsFromRepo);
            Response.AddPagination(examsFromRepo.CurrentPage, examsFromRepo.PageSize, examsFromRepo.TotalItems, examsFromRepo.TotalPages);

            return Ok(examsForReturn);
        }
        [HttpGet("enrolled/{userId}", Name = "GetExamsForUserAsync")]
        public async Task<IActionResult> GetExamsForUserAsync(int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            var examsFromRepo = await _repo.UserExamRepository.GetExamsForUserAsync(userId);
            var examsForReturn = _mapper.Map<IEnumerable<ExamForListDto>>(examsFromRepo);
            return Ok(examsForReturn);

        }

        [Authorize(Policy = "RequireTeacherRole")]
        [HttpPost]
        public async Task<IActionResult> CreateExamAsync(ExamForCreationDto examForCreation)
        {
            var exam = _mapper.Map<Exam>(examForCreation);
            exam.AuthorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            _repo.ExamRepository.Add(exam);
            if (await _repo.SaveAllAsync())
            {
                return CreatedAtRoute("GetExamAsync", new { examId = exam.Id }, exam);
            }
            return BadRequest("Failed to save exam");
        }

        [HttpPost("{examId}/enroll/{userId}")]
        public async Task<IActionResult> AddUserToExamAsync(int userId, int examId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            var examFromRepo = await _repo.ExamRepository.GetExamAsync(examId);
            if (examFromRepo == null)
                return NotFound("Exam not exists");
            var userExam = new UserExam
            {
                UserId = userId,
                ExamId = examId
            };
            _repo.UserExamRepository.Add(userExam);
            if (await _repo.SaveAllAsync())
                return CreatedAtRoute("GetExamsForUserAsync", new { userId = userId }, userExam);

            return BadRequest("Failed to add user to exam");

        }


        [Authorize(Policy = "RequireTeacherRole")]
        [HttpDelete("{examId}")]
        public async Task<IActionResult> DeleteExamAsync(int examId)
        {
            var exam = await _repo.ExamRepository.GetExamAsync(examId);
            if (exam == null)
            {
                return NotFound();
            }
            if (exam.AuthorId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            _repo.ExamRepository.Delete(exam);
            if (await _repo.SaveAllAsync())
            {
                return NoContent();
            }
            return BadRequest("Failed to delete exam");
        }



    }
}