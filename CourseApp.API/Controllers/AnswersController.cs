using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CourseApp.API.Dtos;
using CourseApp.API.IRepositories;
using CourseApp.API.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseApp.API.Controllers
{
    [ApiController]
    [Route("api/exams/questions/{questionId}/[controller]")]
    public class AnswersController : ControllerBase
    {
        private readonly IRepositoryWrapper _repo;
        private readonly IMapper _mapper;
        public AnswersController(IRepositoryWrapper repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;

        }
        [HttpGet("{answerId}", Name = "GetAnswerAsync")]
        public async Task<IActionResult> GetAnswerAsync(int answerId)
        {
            var answerFromRepo = await _repo.AnswerRepository.GetAnswerAsync(answerId);
            if (answerFromRepo == null)
                return NotFound();
            var answerForReturn = _mapper.Map<AnswerForReturnDto>(answerFromRepo);
            return Ok(answerForReturn);
        }
        [HttpGet(Name = "GetAnswersAsync")]
        public async Task<IActionResult> GetAnswersAsync(int questionId)
        {
            var answersFromRepo = await _repo.AnswerRepository.GetAnswersAsync(questionId);
            if (answersFromRepo == null)
                return NotFound();
            var answersForReturn = _mapper.Map<IEnumerable<AnswerForReturnDto>>(answersFromRepo);
            return Ok(answersForReturn);
        }

        [Authorize(Policy = "RequireTeacherRole")]
        [HttpPost]
        public async Task<IActionResult> CreateAnswerAsync(int questionId, AnswerForCreationDto answerforCreation)
        {
            var questionFromRepo = await _repo.QuestionRepository.GetQuestionAsync(questionId);
            if (questionFromRepo == null)
                return BadRequest("Question not found");
            if (questionFromRepo.Exam.AuthorId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            var answer = _mapper.Map<Answer>(answerforCreation);
            answer.QuestionId = questionId;
            _repo.AnswerRepository.Add(answer);
            if (await _repo.SaveAllAsync())
            {
                var answerForReturn = _mapper.Map<AnswerForReturnDto>(answer);
                return CreatedAtRoute("GetAnswerAsync", new { answerId = answer.Id }, answerForReturn);
            }
            return BadRequest("Failed to save an answer");
        }

        [Authorize(Policy = "RequireTeacherRole")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateAnswersAsync(int questionId, IEnumerable<AnswerForCreationDto> answersForCreation)
        {

            var questionFromRepo = await _repo.QuestionRepository.GetQuestionAsync(questionId);
            if (questionFromRepo == null)
                return BadRequest("Question not found");
            if (questionFromRepo.Exam.AuthorId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            var answers = _mapper.Map<IEnumerable<Answer>>(answersForCreation);
            foreach (var answer in answers)
            {
                answer.QuestionId = questionId;
                _repo.AnswerRepository.Add(answer);
            }
            if (await _repo.SaveAllAsync())
            {
                var answersForReturn = _mapper.Map<IEnumerable<AnswerForReturnDto>>(answers);
                return CreatedAtRoute("GetAnswersAsync", new { questionId = questionId }, answersForReturn);
            }
            return BadRequest("Failed to save an answer");
        }

    }
}