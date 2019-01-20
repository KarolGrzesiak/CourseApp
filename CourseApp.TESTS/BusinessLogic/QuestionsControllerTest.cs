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
    public class QuestionsControllerTest
    {
        [Fact]
        public async Task GetQuestionAsync_ValidQuestionIdPassed_ReturnsOkObjectResult()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
{
                    new Claim(ClaimTypes.NameIdentifier, "1"),

      }));
            var mapperMock = new Mock<IMapper>();
            var repositoryMock = new Mock<IRepositoryWrapper>();
            repositoryMock.Setup(r => r.QuestionRepository.GetQuestionAsync(It.IsAny<int>())).ReturnsAsync(new Question());
            mapperMock.Setup(m => m.Map<QuestionForReturnDto>(It.IsAny<Question>())).Returns(new QuestionForReturnDto());
            var controllerMock = new QuestionsController(repositoryMock.Object, mapperMock.Object);
            controllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = user
                }
            };
            var result = await controllerMock.GetQuestionAsync(1);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetQuestionsAsync_ValidExamIdPassed_ReturnsOkObjectResult()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
{
                    new Claim(ClaimTypes.NameIdentifier, "1"),

      }));
            var mapperMock = new Mock<IMapper>();
            var repositoryMock = new Mock<IRepositoryWrapper>();
            repositoryMock.Setup(r => r.QuestionRepository.GetQuestionsAsync(It.IsAny<int>())).ReturnsAsync(new List<Question>());
            mapperMock.Setup(m => m.Map<IEnumerable<QuestionForReturnDto>>(It.IsAny<IEnumerable<Question>>())).Returns(new List<QuestionForReturnDto>());
            var controllerMock = new QuestionsController(repositoryMock.Object, mapperMock.Object);
            controllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = user
                }
            };
            var result = await controllerMock.GetQuestionsAsync(1);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task CreateQuestionAsync_ValidExamIdPassed_ReturnsCreatedAtRouteResult()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
    {
                    new Claim(ClaimTypes.NameIdentifier, "1"),

      }));
            var exam = new Exam()
            {
                AuthorId = 1
            };


            var mapperMock = new Mock<IMapper>();
            var repositoryMock = new Mock<IRepositoryWrapper>();

            repositoryMock.Setup(r => r.ExamRepository.GetExamAsync(It.IsAny<int>())).ReturnsAsync(exam);
            repositoryMock.Setup(r => r.QuestionRepository.Add(It.IsAny<Question>()));
            repositoryMock.Setup(r => r.SaveAllAsync()).ReturnsAsync(true);
            mapperMock.Setup(m => m.Map<Question>(It.IsAny<QuestionForCreationDto>())).Returns(new Question());
            mapperMock.Setup(m => m.Map<QuestionForReturnDto>(It.IsAny<Question>())).Returns(new QuestionForReturnDto());

            var controllerMock = new QuestionsController(repositoryMock.Object, mapperMock.Object);
            controllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = user
                }
            };
            var result = await controllerMock.CreateQuestionAsync(1, new QuestionForCreationDto());
            Assert.IsType<CreatedAtRouteResult>(result);

        }
        [Fact]
        public async Task CreateQuestionAsync_UnauthorizedUserCall_ReturnsUnauthorizedResult()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
    {
                    new Claim(ClaimTypes.NameIdentifier, "1"),

      }));
            var exam = new Exam()
            {
                AuthorId = 2
            };


            var mapperMock = new Mock<IMapper>();
            var repositoryMock = new Mock<IRepositoryWrapper>();

            repositoryMock.Setup(r => r.ExamRepository.GetExamAsync(It.IsAny<int>())).ReturnsAsync(exam);
            repositoryMock.Setup(r => r.QuestionRepository.Add(It.IsAny<Question>()));
            repositoryMock.Setup(r => r.SaveAllAsync()).ReturnsAsync(true);
            mapperMock.Setup(m => m.Map<Question>(It.IsAny<QuestionForCreationDto>())).Returns(new Question());
            mapperMock.Setup(m => m.Map<QuestionForReturnDto>(It.IsAny<Question>())).Returns(new QuestionForReturnDto());

            var controllerMock = new QuestionsController(repositoryMock.Object, mapperMock.Object);
            controllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = user
                }
            };
            var result = await controllerMock.CreateQuestionAsync(1, new QuestionForCreationDto());
            Assert.IsType<UnauthorizedResult>(result);

        }


        [Fact]
        public async Task DeleteQuestionAsync_ValidExamAndQuestionIdPassed_ReturnsNoContentResult()
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
            repositoryMock.Setup(r => r.QuestionRepository.Delete(It.IsAny<Question>()));
            repositoryMock.Setup(r => r.SaveAllAsync()).ReturnsAsync(true);
            var controllerMock = new QuestionsController(repositoryMock.Object, mapperMock.Object);
            controllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = user
                }
            };
            var result = await controllerMock.DeleteQuestionAsync(1,1);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteQuestionAsync_UnauthorizedUserCall_ReturnUnauthorizedResult()
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
            repositoryMock.Setup(r => r.QuestionRepository.Delete(It.IsAny<Question>()));
            repositoryMock.Setup(r => r.SaveAllAsync()).ReturnsAsync(true);
            var controllerMock = new QuestionsController(repositoryMock.Object, mapperMock.Object);
            controllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = user
                }
            };
            var result = await controllerMock.DeleteQuestionAsync(1, 1);
            Assert.IsType<UnauthorizedResult>(result);
        }

    }
}