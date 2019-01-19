using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CourseApp.API.Dtos;
using CourseApp.API.Helpers;
using CourseApp.API.IRepositories;
using CourseApp.API.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
        [HttpGet(Name = "GetNotEnrolledExamsForUserAsync")]
        public async Task<IActionResult> GetNotEnrolledExamsForUserAsync([FromQuery] int? pageNumber, [FromQuery] int? pageSize)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var examsFromRepo = await _repo.ExamRepository.GetNotEnrolledExamsForUserAsync(pageNumber, pageSize, userId);
            var examsForReturn = _mapper.Map<IEnumerable<ExamForListDto>>(examsFromRepo);
            Response.AddPagination(examsFromRepo.CurrentPage, examsFromRepo.PageSize, examsFromRepo.TotalItems, examsFromRepo.TotalPages);

            return Ok(examsForReturn);
        }
        [HttpGet("enrolled", Name = "GetEnrolledExamsForUserAsync")]
        public async Task<IActionResult> GetEnrolledExamsForUserAsync()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var examsFromRepo = await _repo.UserExamRepository.GetEnrolledExamsForUserAsync(userId);
            var examsForReturn = _mapper.Map<IEnumerable<ExamForListDto>>(examsFromRepo);
            return Ok(examsForReturn);

        }
        [Authorize(Policy = "RequireTeacherRole")]
        [HttpGet("created", Name = "GetCreatedExamsForUserAsync")]
        public async Task<IActionResult> GetCreatedExamsForUserAsync()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var examsFromRepo = await _repo.ExamRepository.GetCreatedExamsForUserAsync(userId);
            var examsForReturn = _mapper.Map<IEnumerable<ExamForListDto>>(examsFromRepo);
            return Ok(examsForReturn);
        }

        [HttpGet("finished", Name = "GetFinishedExamsForUserAsync")]
        public async Task<IActionResult> GetFinishedExamsForUserAsync()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var examsFromRepo = await _repo.UserExamRepository.GetFinishedExamsForUserAsync(userId);
            var examsForReturn = _mapper.Map<IEnumerable<ExamForListDto>>(examsFromRepo);
            return Ok(examsForReturn);
        }

        [Authorize(Policy = "RequireTeacherRole")]
        [HttpPost]
        public async Task<IActionResult> CreateExamAsync(ExamForCreationDto examForCreation)
        {
            byte[] passwordHash, passwordSalt;
            var exam = _mapper.Map<Exam>(examForCreation);
            examForCreation.Password.CreatePasswordHash(out passwordHash, out passwordSalt);
            exam.PasswordHash = passwordHash;
            exam.PasswordSalt = passwordSalt;
            exam.AuthorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            _repo.ExamRepository.Add(exam);
            if (await _repo.SaveAllAsync())
            {
                return CreatedAtRoute("GetExamAsync", new { examId = exam.Id }, exam);
            }
            return BadRequest("Failed to save exam");
        }

        [HttpPost("{examId}/enroll/{userId}")]
        public async Task<IActionResult> AddUserToExamAsync(int userId, int examId, [FromBody] string password)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            var examFromRepo = await _repo.ExamRepository.GetExamAsync(examId);
            if (!password.VerifyPasswordHash(examFromRepo.PasswordHash, examFromRepo.PasswordSalt))
                return Unauthorized();
            if (examFromRepo == null)
                return NotFound("Exam not exists");
            var userExam = new UserExam
            {
                UserId = userId,
                ExamId = examId
            };
            _repo.UserExamRepository.Add(userExam);
            if (await _repo.SaveAllAsync())
                return CreatedAtRoute("GetEnrolledExamsForUserAsync", new { userId = userId }, userExam);

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
        [HttpGet("{examId}/userId/answers", Name = "GetUserAnswersAsync")]
        public async Task<IActionResult> GetUserAnswersAsync(int examId, int userId)
        {
            var userAnswers = await _repo.UserAnswerRepository.GetUserAnswersAsync(examId, userId);
            if (userAnswers == null)
                return NotFound();
            return Ok(userAnswers);
        }

        [HttpPost("{examId}/take")]
        public async Task<IActionResult> CreateUserAnswersAsync(int examId, IEnumerable<UserAnswersForCreationDto> userAnswersForCreation)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var userExam = await _repo.UserExamRepository.GetUserWithExamAsync(userId, examId);
            if (userExam == null)
            {
                return Unauthorized();
            }
            if (userExam.Exam.AuthorId == userId)
            {
                return BadRequest("Solving your own test is forbidden");
            }
            userExam.Finished = true;
            _repo.UserExamRepository.Update(userExam);
            var userAnswers = _mapper.Map<IEnumerable<UserAnswer>>(userAnswersForCreation);
            foreach (var userAnswer in userAnswers)
            {
                userAnswer.UserId = userId;
                _repo.UserAnswerRepository.Add(userAnswer);
            }
            if (await _repo.SaveAllAsync())
            {
                return CreatedAtRoute("GetUserAnswersAsync", new { examId = examId, userId = userId }, userAnswers);
            }
            return BadRequest("Failed to save user's answers");

        }




    }
}