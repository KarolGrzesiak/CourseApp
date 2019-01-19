using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CourseApp.API.Controllers;
using CourseApp.API.Dtos;
using CourseApp.API.Helpers;
using CourseApp.API.IRepositories;
using CourseApp.API.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CourseApp.Tests.BusinessLogic
{
    public class ExamsControllerTest
    {
        [Fact]
        public async Task GetExam_ValidExamIdPassed_ReturnsOkObjectResult()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
   {
                    new Claim(ClaimTypes.NameIdentifier, "1"),
        }));
            var mapperMock = new Mock<IMapper>();
            var repositoryMock = new Mock<IRepositoryWrapper>();
            repositoryMock.Setup(r => r.ExamRepository.GetExamAsync(It.IsAny<int>())).ReturnsAsync(new Exam());


            var controllerMock = new ExamsController(repositoryMock.Object, mapperMock.Object);
            controllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = user
                }
            };
            var result = await controllerMock.GetExamAsync(1);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetNotEnrolledExamsForUserAsync_ValidRequest_ReturnsOkObjectResult()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
{
                    new Claim(ClaimTypes.NameIdentifier, "1"),
     }));
            var mapperMock = new Mock<IMapper>();
            var repositoryMock = new Mock<IRepositoryWrapper>();


            repositoryMock.Setup(r => r.ExamRepository.GetNotEnrolledExamsForUserAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(new PagedList<Exam>(new List<Exam>(), 1, 1, 1));
            mapperMock.Setup(m => m.Map<IEnumerable<ExamForListDto>>(It.IsAny<PagedList<Exam>>())).Returns(new List<ExamForListDto>());



            var controllerMock = new ExamsController(repositoryMock.Object, mapperMock.Object);
            controllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = user
                }
            };
            var result = await controllerMock.GetNotEnrolledExamsForUserAsync(1, 1);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetEnrolledExamsForUserAsync_ValidRequest_ReturnsOkObjectResult()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
{
                    new Claim(ClaimTypes.NameIdentifier, "1"),
     }));
            var mapperMock = new Mock<IMapper>();
            var repositoryMock = new Mock<IRepositoryWrapper>();


            repositoryMock.Setup(r => r.UserExamRepository.GetEnrolledExamsForUserAsync(It.IsAny<int>())).ReturnsAsync(new List<Exam>());
            mapperMock.Setup(m => m.Map<IEnumerable<ExamForListDto>>(It.IsAny<IEnumerable<Exam>>())).Returns(new List<ExamForListDto>());



            var controllerMock = new ExamsController(repositoryMock.Object, mapperMock.Object);
            controllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = user
                }
            };
            var result = await controllerMock.GetEnrolledExamsForUserAsync();
            Assert.IsType<OkObjectResult>(result);
        }
        [Fact]
        public async Task GetCreatedExamsForUserAsync_ValidRequest_ReturnsOkObjectResult()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
{
                    new Claim(ClaimTypes.NameIdentifier, "1"),

        }));
            var mapperMock = new Mock<IMapper>();
            var repositoryMock = new Mock<IRepositoryWrapper>();


            repositoryMock.Setup(r => r.ExamRepository.GetCreatedExamsForUserAsync(It.IsAny<int>())).ReturnsAsync(new List<Exam>());
            mapperMock.Setup(m => m.Map<IEnumerable<ExamForListDto>>(It.IsAny<IEnumerable<Exam>>())).Returns(new List<ExamForListDto>());



            var controllerMock = new ExamsController(repositoryMock.Object, mapperMock.Object);
            controllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = user
                }
            };
            var result = await controllerMock.GetCreatedExamsForUserAsync();
            Assert.IsType<OkObjectResult>(result);
        }
        [Fact]
        public async Task GetFinishedExamsForUserAsync_ValidRequest_ReturnsOkObjectResult()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
{
                    new Claim(ClaimTypes.NameIdentifier, "1"),

        }));
            var mapperMock = new Mock<IMapper>();
            var repositoryMock = new Mock<IRepositoryWrapper>();


            repositoryMock.Setup(r => r.UserExamRepository.GetFinishedExamsForUserAsync(It.IsAny<int>())).ReturnsAsync(new List<Exam>());
            mapperMock.Setup(m => m.Map<IEnumerable<ExamForListDto>>(It.IsAny<IEnumerable<Exam>>())).Returns(new List<ExamForListDto>());



            var controllerMock = new ExamsController(repositoryMock.Object, mapperMock.Object);
            controllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = user
                }
            };
            var result = await controllerMock.GetFinishedExamsForUserAsync();
            Assert.IsType<OkObjectResult>(result);
        }



        [Fact]
        public async Task CreateExamAsync_ValidRequest_ReturnsCreatedAtRouteResult()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
{
                    new Claim(ClaimTypes.NameIdentifier, "1"),

        }));
            var examForCreation = new ExamForCreationDto()
            {
                Password = "test"
            };

            var mapperMock = new Mock<IMapper>();
            var repositoryMock = new Mock<IRepositoryWrapper>();


            repositoryMock.Setup(r => r.SaveAllAsync()).ReturnsAsync(true);
            repositoryMock.Setup(r => r.ExamRepository.Add(It.IsAny<Exam>()));
            mapperMock.Setup(m => m.Map<Exam>(It.IsAny<ExamForCreationDto>())).Returns(new Exam());



            var controllerMock = new ExamsController(repositoryMock.Object, mapperMock.Object);
            controllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = user
                }
            };
            var result = await controllerMock.CreateExamAsync(examForCreation);
            Assert.IsType<CreatedAtRouteResult>(result);
        }

        [Fact]
        public async Task CreateExamAsync_InvalidRequest_ReturnsBadRequestResult()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
{
                    new Claim(ClaimTypes.NameIdentifier, "1"),

        }));
            var examForCreation = new ExamForCreationDto()
            {
                Password = "test"
            };

            var mapperMock = new Mock<IMapper>();
            var repositoryMock = new Mock<IRepositoryWrapper>();


            repositoryMock.Setup(r => r.SaveAllAsync()).ReturnsAsync(false);
            repositoryMock.Setup(r => r.ExamRepository.Add(It.IsAny<Exam>()));
            mapperMock.Setup(m => m.Map<Exam>(It.IsAny<ExamForCreationDto>())).Returns(new Exam());



            var controllerMock = new ExamsController(repositoryMock.Object, mapperMock.Object);
            controllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = user
                }
            };
            var result = await controllerMock.CreateExamAsync(examForCreation);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task AddUserToExamAsync_InvalidPasswordPassed_ReturnsUnauthorizedResult()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
{
                    new Claim(ClaimTypes.NameIdentifier, "1"),

      }));
            string password = "test";
            var mapperMock = new Mock<IMapper>();
            var repositoryMock = new Mock<IRepositoryWrapper>();


            repositoryMock.Setup(r => r.ExamRepository.GetExamAsync(It.IsAny<int>())).ReturnsAsync(new Exam()
            {
                PasswordHash = new byte[5],
                PasswordSalt = new byte[5]
            });
            mapperMock.Setup(m => m.Map<IEnumerable<ExamForListDto>>(It.IsAny<IEnumerable<Exam>>())).Returns(new List<ExamForListDto>());



            var controllerMock = new ExamsController(repositoryMock.Object, mapperMock.Object);
            controllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = user
                }
            };
            var result = await controllerMock.AddUserToExamAsync(1, 1, password);
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task AddUserToExamAsync_ValidParametersPassed_ReturnsCreatedAtRouteResult()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
{
                    new Claim(ClaimTypes.NameIdentifier, "1"),

      }));
            byte[] passwordHash, passwordSalt;
            string password = "test";
            var mapperMock = new Mock<IMapper>();
            var repositoryMock = new Mock<IRepositoryWrapper>();
            password.CreatePasswordHash(out passwordHash, out passwordSalt);

            repositoryMock.Setup(r => r.ExamRepository.GetExamAsync(It.IsAny<int>())).ReturnsAsync(new Exam()
            {
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            });
            repositoryMock.Setup(r => r.UserExamRepository.Add(It.IsAny<UserExam>()));
            repositoryMock.Setup(r => r.SaveAllAsync()).ReturnsAsync(true);



            var controllerMock = new ExamsController(repositoryMock.Object, mapperMock.Object);
            controllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = user
                }
            };
            var result = await controllerMock.AddUserToExamAsync(1, 1, password);
            Assert.IsType<CreatedAtRouteResult>(result);
        }

        [Fact]
        public async Task DeleteExamAsync_ValidExamIdPassed_ReturnsNoContentResult()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
{
                    new Claim(ClaimTypes.NameIdentifier, "1"),

       }));
            var mapperMock = new Mock<IMapper>();
            var repositoryMock = new Mock<IRepositoryWrapper>();


            repositoryMock.Setup(r => r.ExamRepository.GetExamAsync(It.IsAny<int>())).ReturnsAsync(new Exam()
            {
                AuthorId = 1
            });
            repositoryMock.Setup(r => r.ExamRepository.Delete(It.IsAny<Exam>()));
            repositoryMock.Setup(r => r.SaveAllAsync()).ReturnsAsync(true);



            var controllerMock = new ExamsController(repositoryMock.Object, mapperMock.Object);
            controllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = user
                }
            };
            var result = await controllerMock.DeleteExamAsync(1);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteExamAsync_NotAuthorizedCall_ReturnsUnauthorizedResult()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
{
                    new Claim(ClaimTypes.NameIdentifier, "1"),

       }));
            var mapperMock = new Mock<IMapper>();
            var repositoryMock = new Mock<IRepositoryWrapper>();


            repositoryMock.Setup(r => r.ExamRepository.GetExamAsync(It.IsAny<int>())).ReturnsAsync(new Exam()
            {
                AuthorId = 2
            });
            repositoryMock.Setup(r => r.ExamRepository.Delete(It.IsAny<Exam>()));
            repositoryMock.Setup(r => r.SaveAllAsync()).ReturnsAsync(true);



            var controllerMock = new ExamsController(repositoryMock.Object, mapperMock.Object);
            controllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = user
                }
            };
            var result = await controllerMock.DeleteExamAsync(1);
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task GetUserAnswersAsync_ValidExamAndUserIdPassed_ReturnsOkObjectResult()
        {
            var mapperMock = new Mock<IMapper>();
            var repositoryMock = new Mock<IRepositoryWrapper>();
            repositoryMock.Setup(r => r.UserAnswerRepository.GetUserAnswersAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(new List<UserAnswer>());
            var controllerMock = new ExamsController(repositoryMock.Object, mapperMock.Object);
            var result = await controllerMock.GetUserAnswersAsync(1, 1);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task CreateUserAnswerAsync_ValidParametersPassed_ReturnsCreatedAtRouteResult()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
{
                    new Claim(ClaimTypes.NameIdentifier, "1"),

  }));
            var exam = new Exam
            {
                AuthorId = 2
            };
            var userExam = new UserExam
            {
                Exam = exam
            };
            var mapperMock = new Mock<IMapper>();
            var repositoryMock = new Mock<IRepositoryWrapper>();

            repositoryMock.Setup(r => r.UserExamRepository.GetUserWithExamAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(userExam);
            repositoryMock.Setup(r => r.UserExamRepository.Update(It.IsAny<UserExam>()));
            repositoryMock.Setup(r => r.UserAnswerRepository.Add(It.IsAny<UserAnswer>()));
            repositoryMock.Setup(r => r.SaveAllAsync()).ReturnsAsync(true);
            mapperMock.Setup(m => m.Map<IEnumerable<UserAnswer>>(It.IsAny<IEnumerable<UserAnswersForCreationDto>>())).Returns(new List<UserAnswer>());


            var controllerMock = new ExamsController(repositoryMock.Object, mapperMock.Object);
            controllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = user
                }
            };
            var result = await controllerMock.CreateUserAnswersAsync(1, new List<UserAnswersForCreationDto>());
            Assert.IsType<CreatedAtRouteResult>(result);

        }

        [Fact]
        public async Task CreateUserAnswerAsync_ExamAuthorRequest_ReturnsBadRequestResult()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
    {
                    new Claim(ClaimTypes.NameIdentifier, "1"),

    }));
            var exam = new Exam
            {
                AuthorId = 1
            };
            var userExam = new UserExam
            {
                Exam = exam
            };
            var mapperMock = new Mock<IMapper>();
            var repositoryMock = new Mock<IRepositoryWrapper>();

            repositoryMock.Setup(r => r.UserExamRepository.GetUserWithExamAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(userExam);
            repositoryMock.Setup(r => r.UserExamRepository.Update(It.IsAny<UserExam>()));
            repositoryMock.Setup(r => r.UserAnswerRepository.Add(It.IsAny<UserAnswer>()));
            repositoryMock.Setup(r => r.SaveAllAsync()).ReturnsAsync(true);
            mapperMock.Setup(m => m.Map<IEnumerable<UserAnswer>>(It.IsAny<IEnumerable<UserAnswersForCreationDto>>())).Returns(new List<UserAnswer>());


            var controllerMock = new ExamsController(repositoryMock.Object, mapperMock.Object);
            controllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = user
                }
            };
            var result = await controllerMock.CreateUserAnswersAsync(1, new List<UserAnswersForCreationDto>());
            Assert.IsType<BadRequestObjectResult>(result);

        }
        
    }
}