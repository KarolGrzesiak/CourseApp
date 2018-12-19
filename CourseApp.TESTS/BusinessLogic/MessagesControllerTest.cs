
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using AutoMapper;
using CourseApp.API.Controllers;
using CourseApp.API.IRepositories;
using CourseApp.API.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CourseApp.Tests.BusinessLogic
{
    public class MessagesControllerTest
    {
        [Fact]
        public async Task GetMessage_NotExistingMessageIdPassed_ReturnsNotFound()
        {

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
                    new Claim(ClaimTypes.NameIdentifier, "1"),
                }));


            var repositoryMock = new Mock<IRepositoryWrapper>();
            repositoryMock.Setup(r => r.MessageRepository.GetMessageAsync(0)).ReturnsAsync(() => null);
            var mapperMock = new Mock<IMapper>();

            var controllerMock = new MessagesController(repositoryMock.Object, mapperMock.Object);
            controllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = user
                }
            };

            var result = await controllerMock.GetMessage(1, 0);

            Assert.IsType<NotFoundResult>(result);
        }


        [Fact]
        public async Task GetMessage_ExistingMessageIdPassed_ReturnsOkResult()
        {

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
                    new Claim(ClaimTypes.NameIdentifier, "1"),
                }));


            var repositoryMock = new Mock<IRepositoryWrapper>();
            var messageToReturn = new Message
            {
                Id = 1
            };
            repositoryMock.Setup(r => r.MessageRepository.GetMessageAsync(1)).ReturnsAsync(() => messageToReturn);
            var mapperMock = new Mock<IMapper>();

            var controllerMock = new MessagesController(repositoryMock.Object, mapperMock.Object);
            controllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = user
                }
            };

            var result = await controllerMock.GetMessage(1, 1) as OkObjectResult;

            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(messageToReturn, result.Value);
        }
    }
}