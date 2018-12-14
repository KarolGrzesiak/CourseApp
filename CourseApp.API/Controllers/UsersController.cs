using System.Threading.Tasks;
using CourseApp.API.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IRepositoryWrapper _repo;
        public UsersController(IRepositoryWrapper repo)
        {
            _repo = repo;

        }
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _repo.UserRepository.GetUsersAsync();

            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserAsync(int id){
            var user = await _repo.UserRepository.GetUserAsync(id);
            return Ok(user);
        }
    }
}