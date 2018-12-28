using System;
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
            var examsFromRepo = await _repo.ExamRepository.GetExamsAsync(pageNumber, pageSize);
            Response.AddPagination(examsFromRepo.CurrentPage, examsFromRepo.PageSize, examsFromRepo.TotalItems, examsFromRepo.TotalPages);
            return Ok(examsFromRepo);
        }

        [Authorize(Policy = "RequireTeacherRole")]
        [HttpPost]
        public async Task<IActionResult> CreateExamAsync(ExamForCreationDto examForCreation)
        {
            var exam = _mapper.Map<Exam>(examForCreation);
            exam.AuthorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            _repo.ExamRepository.Add(exam);
            if(await _repo.SaveAllAsync()){
                return CreatedAtRoute("GetExamAsync", new { examId = exam.Id }, exam);
            }
            return BadRequest("Failed to save exam");
        }


        [Authorize(Policy="RequireTeacherRole")]
        [HttpDelete("{examId}")]
        public async Task<IActionResult> DeleteExamAsync(int examId){
            var exam = await _repo.ExamRepository.GetExamAsync(examId);
            if(exam==null){
                return NotFound();
            }
            if (exam.AuthorId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            _repo.ExamRepository.Delete(exam);
            if(await _repo.SaveAllAsync()){
                return NoContent();
            }
            return BadRequest("Failed to delete exam");
        }



    }
}