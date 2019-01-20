using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CourseApp.API.Controllers;
using CourseApp.API.Dtos;
using CourseApp.API.IRepositories;
using CourseApp.API.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CourseApp.Tests.BusinessLogic
{
    public class AnswersControllerTest
    {
        [Fact]
        public async Task GetAnswerAsync_ValidAnswerIdPassed_ReturnsOkObjectResult()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
{
                    new Claim(ClaimTypes.NameIdentifier, "1"),

      }));
            var mapperMock = new Mock<IMapper>();
            var repositoryMock = new Mock<IRepositoryWrapper>();
            repositoryMock.Setup(r => r.AnswerRepository.GetAnswerAsync(It.IsAny<int>())).ReturnsAsync(new Answer());
            mapperMock.Setup(m => m.Map<AnswerForReturnDto>(It.IsAny<Answer>())).Returns(new AnswerForReturnDto());
            var controllerMock = new AnswersController(repositoryMock.Object, mapperMock.Object);
            controllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = user
                }
            };
            var result = await controllerMock.GetAnswerAsync(1);
            Assert.IsType<OkObjectResult>(result);
        }
        [Fact]
        public async Task GetAnswersAsync_ValidQuestionIdPassed_ReturnsOkObjectResult()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
{
                    new Claim(ClaimTypes.NameIdentifier, "1"),

      }));
            var mapperMock = new Mock<IMapper>();
            var repositoryMock = new Mock<IRepositoryWrapper>();
            repositoryMock.Setup(r => r.AnswerRepository.GetAnswersAsync(It.IsAny<int>())).ReturnsAsync(new List<Answer>());
            mapperMock.Setup(m => m.Map<IEnumerable<AnswerForReturnDto>>(It.IsAny<IEnumerable<Answer>>())).Returns(new List<AnswerForReturnDto>());
            var controllerMock = new AnswersController(repositoryMock.Object, mapperMock.Object);
            controllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = user
                }
            };
            var result = await controllerMock.GetAnswersAsync(1);
            Assert.IsType<OkObjectResult>(result);
        }
        [Fact]
        public async Task CreateAnswerAsync_ValidQuestionIdPassed_ReturnsCreatedAtRouteResult()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
{
                    new Claim(ClaimTypes.NameIdentifier, "1"),

      }));
            var exam = new Exam()
            {
                AuthorId = 1
            };

            var question = new Question()
            {
                Exam = exam
            };
            var mapperMock = new Mock<IMapper>();
            var repositoryMock = new Mock<IRepositoryWrapper>();

            repositoryMock.Setup(r => r.QuestionRepository.GetQuestionAsync(It.IsAny<int>())).ReturnsAsync(question);
            repositoryMock.Setup(r => r.AnswerRepository.Add(It.IsAny<Answer>()));
            repositoryMock.Setup(r => r.SaveAllAsync()).ReturnsAsync(true);
            mapperMock.Setup(m => m.Map<Answer>(It.IsAny<AnswerForCreationDto>())).Returns(new Answer());
            mapperMock.Setup(m => m.Map<AnswerForReturnDto>(It.IsAny<Answer>())).Returns(new AnswerForReturnDto());

            var controllerMock = new AnswersController(repositoryMock.Object, mapperMock.Object);
            controllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = user
                }
            };
            var result = await controllerMock.CreateAnswerAsync(1, new AnswerForCreationDto());
            Assert.IsType<CreatedAtRouteResult>(result);

        }
        [Fact]
        public async Task CreateAnswerAsync_InvalidQuestionIdPassed_ReturnsUnauthorizedResult()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
{
                    new Claim(ClaimTypes.NameIdentifier, "1"),

      }));
            var exam = new Exam()
            {
                AuthorId = 2
            };

            var question = new Question()
            {
                Exam = exam
            };
            var mapperMock = new Mock<IMapper>();
            var repositoryMock = new Mock<IRepositoryWrapper>();

            repositoryMock.Setup(r => r.QuestionRepository.GetQuestionAsync(It.IsAny<int>())).ReturnsAsync(question);
            repositoryMock.Setup(r => r.AnswerRepository.Add(It.IsAny<Answer>()));
            repositoryMock.Setup(r => r.SaveAllAsync()).ReturnsAsync(true);
            mapperMock.Setup(m => m.Map<Answer>(It.IsAny<AnswerForCreationDto>())).Returns(new Answer());
            mapperMock.Setup(m => m.Map<AnswerForReturnDto>(It.IsAny<Answer>())).Returns(new AnswerForReturnDto());

            var controllerMock = new AnswersController(repositoryMock.Object, mapperMock.Object);
            controllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = user
                }
            };
            var result = await controllerMock.CreateAnswerAsync(1, new AnswerForCreationDto());
            Assert.IsType<UnauthorizedResult>(result);

        }

        [Fact]
        public async Task CreateAnswersAsync_ValidQuestionIdPassed_ReturnsCreatedAtRouteResult()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
{
                    new Claim(ClaimTypes.NameIdentifier, "1"),

      }));
            var exam = new Exam()
            {
                AuthorId = 1
            };

            var question = new Question()
            {
                Exam = exam
            };
            var mapperMock = new Mock<IMapper>();
            var repositoryMock = new Mock<IRepositoryWrapper>();

            repositoryMock.Setup(r => r.QuestionRepository.GetQuestionAsync(It.IsAny<int>())).ReturnsAsync(question);
            repositoryMock.Setup(r => r.AnswerRepository.Add(It.IsAny<Answer>()));
            repositoryMock.Setup(r => r.SaveAllAsync()).ReturnsAsync(true);
            mapperMock.Setup(m => m.Map<IEnumerable<Answer>>(It.IsAny<IEnumerable<AnswerForCreationDto>>())).Returns(new List<Answer>());
            mapperMock.Setup(m => m.Map<IEnumerable<AnswerForReturnDto>>(It.IsAny<IEnumerable<Answer>>())).Returns(new List<AnswerForReturnDto>());

            var controllerMock = new AnswersController(repositoryMock.Object, mapperMock.Object);
            controllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = user
                }
            };
            var result = await controllerMock.CreateAnswersAsync(1, new List<AnswerForCreationDto>());
            Assert.IsType<CreatedAtRouteResult>(result);

        }
        [Fact]
        public async Task CreateAnswersAsync_UnauthorizedQuestionIdPassed_ReturnsUnauthorizedResult()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
{
                    new Claim(ClaimTypes.NameIdentifier, "1"),

      }));
            var exam = new Exam()
            {
                AuthorId = 2
            };

            var question = new Question()
            {
                Exam = exam
            };
            var mapperMock = new Mock<IMapper>();
            var repositoryMock = new Mock<IRepositoryWrapper>();

            repositoryMock.Setup(r => r.QuestionRepository.GetQuestionAsync(It.IsAny<int>())).ReturnsAsync(question);
            repositoryMock.Setup(r => r.AnswerRepository.Add(It.IsAny<Answer>()));
            repositoryMock.Setup(r => r.SaveAllAsync()).ReturnsAsync(true);
            mapperMock.Setup(m => m.Map<IEnumerable<Answer>>(It.IsAny<IEnumerable<AnswerForCreationDto>>())).Returns(new List<Answer>());
            mapperMock.Setup(m => m.Map<IEnumerable<AnswerForReturnDto>>(It.IsAny<IEnumerable<Answer>>())).Returns(new List<AnswerForReturnDto>());

            var controllerMock = new AnswersController(repositoryMock.Object, mapperMock.Object);
            controllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = user
                }
            };
            var result = await controllerMock.CreateAnswersAsync(1, new List<AnswerForCreationDto>());
            Assert.IsType<UnauthorizedResult>(result);

        }
    }
}