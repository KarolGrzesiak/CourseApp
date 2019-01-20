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
    public class StatsControllerTest
    {
        [Fact]
        public async Task GetStatForUserAsync_ValidExamIdPassed_ReturnsOkObjectResult()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
{
                    new Claim(ClaimTypes.NameIdentifier, "1"),

    }));
            var mapperMock = new Mock<IMapper>();
            var repositoryMock = new Mock<IRepositoryWrapper>();

            repositoryMock.Setup(r => r.UserAnswerRepository.GetUserAnswersAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(new List<UserAnswer>());

            var controllerMock = new StatsController(repositoryMock.Object, mapperMock.Object);
            controllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = user
                }
            };
            var result = await controllerMock.GetStatForUserAsync(1);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetStatsForTeacherAsync_ValidExamIdPassed_ReturnsOkObjectResult()
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

            repositoryMock.Setup(r => r.UserExamRepository.GetUsersFromExamAsync(It.IsAny<int>())).ReturnsAsync(new List<User>());
            repositoryMock.Setup(r => r.ExamRepository.GetExamAsync(It.IsAny<int>())).ReturnsAsync(exam);


            var controllerMock = new StatsController(repositoryMock.Object, mapperMock.Object);
            controllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = user
                }
            };
            var result = await controllerMock.GetStatsForTeacherAsync(1);
            Assert.IsType<OkObjectResult>(result);
        }


        [Fact]
        public async Task GetStatsForTeacherAsync_UnauthorizedUserCall_ReturnsUnauthorizedResult()
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

            repositoryMock.Setup(r => r.UserExamRepository.GetUsersFromExamAsync(It.IsAny<int>())).ReturnsAsync(new List<User>());
            repositoryMock.Setup(r => r.ExamRepository.GetExamAsync(It.IsAny<int>())).ReturnsAsync(exam);


            var controllerMock = new StatsController(repositoryMock.Object, mapperMock.Object);
            controllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = user
                }
            };
            var result = await controllerMock.GetStatsForTeacherAsync(1);
            Assert.IsType<UnauthorizedResult>(result);
        }
    }
}