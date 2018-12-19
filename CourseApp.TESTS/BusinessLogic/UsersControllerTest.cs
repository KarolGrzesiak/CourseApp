using System.Threading.Tasks;
using Xunit;
using Moq;
using CourseApp.API.IRepositories;
using CourseApp.API.Model;
using CourseApp.API.Controllers;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using CourseApp.API.Dtos;

namespace CourseApp.Tests.BusinessLogic
{
    public class UsersControllerTest
    {

        [Fact]
        public async Task GetUserAsync_ExistingIdPassed_ReturnsOkResult()
        {

            var user = new User
            {
                Id = 1
            };
            var repositoryMock = new Mock<IRepositoryWrapper>();
            repositoryMock.Setup(r => r.UserRepository.GetUserAsync(1)).ReturnsAsync(() => user);
            var mapperMock = new Mock<IMapper>();
            var userToReturn = new UserForDetailedDto { Id = 1 };
            mapperMock.Setup(m => m.Map<UserForDetailedDto>(user)).Returns(() => userToReturn);
            var controllerMock = new UsersController(repositoryMock.Object, mapperMock.Object);

            var result = await controllerMock.GetUserAsync(1) as OkObjectResult;

            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(userToReturn, result.Value);
        }
        [Fact]
        public async Task GetUserAsync_NotExistingIdPassed_ReturnsNotFound()
        {


            var repositoryMock = new Mock<IRepositoryWrapper>();
            repositoryMock.Setup(r => r.UserRepository.GetUserAsync(0)).ReturnsAsync(() => null);
            var mapperMock = new Mock<IMapper>();

            var controllerMock = new UsersController(repositoryMock.Object, mapperMock.Object);

            var result = await controllerMock.GetUserAsync(0);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}