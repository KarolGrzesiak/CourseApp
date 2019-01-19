using System.Threading.Tasks;
using Xunit;
using Moq;
using CourseApp.API.IRepositories;
using CourseApp.API.Model;
using CourseApp.API.Controllers;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using CourseApp.API.Dtos;
using CourseApp.API.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

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

        [Fact]
        public async Task GetUsersAsync_ValidRequest_ReturnsOkObjectResult()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
 {
                    new Claim(ClaimTypes.NameIdentifier, "1"),
      }));
            var userParams = new UserParams();

            var repositoryMock = new Mock<IRepositoryWrapper>();
            repositoryMock.Setup(r => r.UserRepository.GetUserAsync(It.IsAny<int>())).ReturnsAsync(new User());
            repositoryMock.Setup(r => r.UserRepository.GetUsersAsync(It.IsAny<UserParams>())).ReturnsAsync(new PagedList<User>(new List<User>(), 1, 1, 1));
            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<IEnumerable<UserForListDto>>(It.IsAny<PagedList<User>>())).Returns(new List<UserForListDto>());

            var controllerMock = new UsersController(repositoryMock.Object, mapperMock.Object);

            controllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = user
                }
            };



            var result = await controllerMock.GetUsersAsync(userParams);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task UpdateUser_ValidUserIdPassed_ReturnsNoContentResult()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
{
                    new Claim(ClaimTypes.NameIdentifier, "1"),
}));

            var repositoryMock = new Mock<IRepositoryWrapper>();
            var mapperMock = new Mock<IMapper>();
            repositoryMock.Setup(r => r.UserRepository.GetUserAsync(It.IsAny<int>())).ReturnsAsync(new User());
            mapperMock.Setup(m => m.Map(It.IsAny<UserForUpdateDto>(), It.IsAny<User>())).Returns(new User());
            repositoryMock.Setup(r => r.SaveAllAsync()).ReturnsAsync(true);
            var controllerMock = new UsersController(repositoryMock.Object, mapperMock.Object);

            controllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = user
                }
            };
            var result = await controllerMock.UpdateUser(1,new UserForUpdateDto());

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateUser_UnauthorizedUserIdPassed_ReturnsUnauthorizedResult()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
{
                    new Claim(ClaimTypes.NameIdentifier, "1"),
}));

            var repositoryMock = new Mock<IRepositoryWrapper>();
            var mapperMock = new Mock<IMapper>();
            repositoryMock.Setup(r => r.UserRepository.GetUserAsync(It.IsAny<int>())).ReturnsAsync(new User());
            mapperMock.Setup(m => m.Map(It.IsAny<UserForUpdateDto>(), It.IsAny<User>())).Returns(new User());
            repositoryMock.Setup(r => r.SaveAllAsync()).ReturnsAsync(true);
            var controllerMock = new UsersController(repositoryMock.Object, mapperMock.Object);

            controllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = user
                }
            };
            var result = await controllerMock.UpdateUser(2, new UserForUpdateDto());

            Assert.IsType<UnauthorizedResult>(result);
        }
    }
}