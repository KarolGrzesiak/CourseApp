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
    [Route("api/exams/{examId}/[controller]")]
    public class QuestionsController : ControllerBase
    {
        private readonly IRepositoryWrapper _repo;
        private readonly IMapper _mapper;
        public QuestionsController(IRepositoryWrapper repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;

        }

        [HttpGet("{questionId}", Name = "GetQuestionAsync")]
        public async Task<IActionResult> GetQuestionAsync(int examId, int questionId)
        {
            var questionFromRepo = await _repo.QuestionRepository.GetQuestionAsync(examId, questionId);
            if (questionFromRepo == null)
                return NotFound();
            var questionForReturn = _mapper.Map<QuestionForReturnDto>(questionFromRepo);
            return Ok(questionForReturn);
        }
        [HttpGet(Name = "GetQuestionsAsync")]
        public async Task<IActionResult> GetQuestionsAsync(int examId)
        {
            var questionsFromRepo = await _repo.QuestionRepository.GetQuestionsAsync(examId);
            var questionsForReturn = _mapper.Map<IEnumerable<QuestionForReturnDto>>(questionsFromRepo);
            return Ok(questionsForReturn);
        }

        [Authorize(Policy = "RequireTeacherRole")]
        [HttpPost]
        public async Task<IActionResult> CreateQuestion(int examId, QuestionForCreationDto questionForCreation)
        {
            var exam = await _repo.ExamRepository.GetExamAsync(examId);
            if (exam == null)
                return BadRequest("Exam not exists");
            if (exam.AuthorId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            var question = _mapper.Map<Question>(questionForCreation);
            question.ExamId = exam.Id;
            _repo.QuestionRepository.Add(question);
            if (await _repo.SaveAllAsync())
            {

                var questionForReturn = _mapper.Map<QuestionForReturnDto>(question);
                return CreatedAtRoute("GetQuestionAsync", new { examId = examId, questionId = question.Id }, questionForReturn);
            }

            return BadRequest("Failed to save a question");


        }

        [Authorize(Policy = "RequireTeacherRole")]
        [HttpDelete("{questionId}")]
        public async Task<IActionResult> DeleteQuestion(int examId, int questionId)
        {
            var question = await _repo.QuestionRepository.GetQuestionAsync(examId, questionId);
            if (question == null)
                return BadRequest("Question not exists");
            if (question.Exam.AuthorId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            _repo.QuestionRepository.Delete(question);
            if (await _repo.SaveAllAsync())
            {
                return NoContent();
            }
            return BadRequest("Failed to delete a question");
        }


    }
}