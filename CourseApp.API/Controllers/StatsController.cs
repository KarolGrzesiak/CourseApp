using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CourseApp.API.Dtos;
using CourseApp.API.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseApp.API.Controllers
{
    [Route("api/exams/[controller]")]
    [ApiController]
    public class StatsController : ControllerBase
    {
        private readonly IRepositoryWrapper _repo;
        private readonly IMapper _mapper;
        public StatsController(IRepositoryWrapper repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;

        }
        [HttpGet("{examId}")]
        public async Task<IActionResult> GetStatForUserAsync(int examId)
        {
            StatsDto statsDto = new StatsDto
            {
                NumberOfCorrectAnswers = 0,
                NumberOfWrongAnswers = 0
            };

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var userAnwers = await _repo.UserAnswerRepository.GetUserAnswersAsync(examId, userId);
            foreach (var userAnswer in userAnwers)
            {
                if (userAnswer.Content == userAnswer.Question.Answers.FirstOrDefault(a => a.isCorrect).Content)
                {
                    statsDto.NumberOfCorrectAnswers++;
                }
                else
                {
                    statsDto.NumberOfWrongAnswers++;
                }
            }
            return Ok(statsDto);
        }
        [Authorize(Policy = "RequireTeacherRole")]
        [HttpGet("teacher/{examId}")]
        public async Task<IActionResult> GetStatsForTeacherAsync(int examId)
        {
            var exam = await _repo.ExamRepository.GetExamAsync(examId);
            if(exam.AuthorId!=int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            List<StatsDto> stats = new List<StatsDto>();
            var users = await _repo.UserExamRepository.GetUsersFromExamAsync(examId);
            users = users.Where(u => u.Id != exam.AuthorId);
            foreach (var user in users)
            {
                StatsDto stat = new StatsDto()
                {
                    NumberOfCorrectAnswers = 0,
                    NumberOfWrongAnswers = 0
                };
                stat.UserName = user.KnownAs;
                var userAnwers = await _repo.UserAnswerRepository.GetUserAnswersAsync(examId, user.Id);
                foreach (var userAnswer in userAnwers)
                {
                    if (userAnswer.Content == userAnswer.Question.Answers.FirstOrDefault(a => a.isCorrect).Content)
                    {
                        stat.NumberOfCorrectAnswers++;
                    }
                    else
                    {
                        stat.NumberOfWrongAnswers++;
                    }
                }
                stats.Add(stat);

            }
            return Ok(stats);


        }



    }
}