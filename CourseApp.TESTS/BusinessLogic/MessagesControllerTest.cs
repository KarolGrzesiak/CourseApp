
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
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


        [Fact]
        public async Task GetMessageForUser_ValidUserIdPassed_ReturnsOkResult()
        {
            var messages = new List<Message>{
                new Message{
                    SenderId=1,
                    RecipientId=1,
                    Content ="test"
        }
            };
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
         {
                    new Claim(ClaimTypes.NameIdentifier, "1"),
                 }));
            var repositoryMock = new Mock<IRepositoryWrapper>();
            var messageParams = new MessageParams();
            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<IEnumerable<MessageToReturnDto>>(It.IsAny<PagedList<Message>>())).Returns(new List<MessageToReturnDto>());

            repositoryMock.Setup(r => r.MessageRepository.GetMessagesForUserAsync(messageParams)).ReturnsAsync(new
            PagedList<Message>(messages, 1, 1, 1));

            var controllerMock = new MessagesController(repositoryMock.Object, mapperMock.Object);
            controllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = user
                }
            };

            var result = await controllerMock.GetMessagesForUser(1, messageParams);
            Assert.IsType<OkObjectResult>(result);
        }


        [Fact]
        public async Task GetMessageForUser_InvalidUserIdPassed_ReturnsUnauthorizedResult()
        {
            var messages = new List<Message>{
                new Message{
                    SenderId=1,
                    RecipientId=1,
                    Content ="test"
            }
            };
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                    new Claim(ClaimTypes.NameIdentifier, "1"),
                 }));
            var repositoryMock = new Mock<IRepositoryWrapper>();
            var messageParams = new MessageParams();
            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<IEnumerable<MessageToReturnDto>>(It.IsAny<PagedList<Message>>())).Returns(new List<MessageToReturnDto>());

            repositoryMock.Setup(r => r.MessageRepository.GetMessagesForUserAsync(messageParams)).ReturnsAsync(new
            PagedList<Message>(messages, 1, 1, 1));

            var controllerMock = new MessagesController(repositoryMock.Object, mapperMock.Object);
            controllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = user
                }
            };

            var result = await controllerMock.GetMessagesForUser(2, messageParams);
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task GetMessageThread_ValidRequest_ReturnsOkResult()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
       {
                    new Claim(ClaimTypes.NameIdentifier, "1"),
            }));
            var mapperMock = new Mock<IMapper>();
            var repositoryMock = new Mock<IRepositoryWrapper>();
            repositoryMock.Setup(r => r.MessageRepository.GetMessageThreadAsync(1, 1)).ReturnsAsync(new List<Message>());
            mapperMock.Setup(m => m.Map<IEnumerable<MessageToReturnDto>>(It.IsAny<List<Message>>())).Returns(new List<MessageToReturnDto>());

            var controllerMock = new MessagesController(repositoryMock.Object, mapperMock.Object);
            controllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = user
                }
            };
            var result = await controllerMock.GetMessageThread(1, 1);
            Assert.IsType<OkObjectResult>(result);
        }
        [Fact]
        public async Task GetMessageThread_InvalidRequest_ReturnsUnauthorizedResult()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
       {
                    new Claim(ClaimTypes.NameIdentifier, "1"),
            }));
            var mapperMock = new Mock<IMapper>();
            var repositoryMock = new Mock<IRepositoryWrapper>();
            repositoryMock.Setup(r => r.MessageRepository.GetMessageThreadAsync(1, 1)).ReturnsAsync(new List<Message>());
            mapperMock.Setup(m => m.Map<IEnumerable<MessageToReturnDto>>(It.IsAny<List<Message>>())).Returns(new List<MessageToReturnDto>());

            var controllerMock = new MessagesController(repositoryMock.Object, mapperMock.Object);
            controllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = user
                }
            };
            var result = await controllerMock.GetMessageThread(2, 1);
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task CreateMessage_ValidRequest_ReturnsCreatedAtRouteResult()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
   {
                    new Claim(ClaimTypes.NameIdentifier, "1"),
        }));
            var messageForCreation = new MessageForCreationDto()
            {

                SenderId = 1,
                RecipientId = 1
            };
            var mapperMock = new Mock<IMapper>();
            var repositoryMock = new Mock<IRepositoryWrapper>();
            repositoryMock.Setup(r => r.UserRepository.GetUserAsync(1)).ReturnsAsync(new User
            {
                Id = 1
            });
            repositoryMock.Setup(r => r.UserRepository.GetUserAsync(2)).ReturnsAsync(new User
            {
                Id = 2
            });
            repositoryMock.Setup(r => r.SaveAllAsync()).ReturnsAsync(true);
            repositoryMock.Setup(r => r.MessageRepository.Add(It.IsAny<Message>()));
            mapperMock.Setup(m => m.Map<Message>(It.IsAny<MessageForCreationDto>())).Returns(new Message());
            mapperMock.Setup(m => m.Map<MessageToReturnDto>(It.IsAny<Message>())).Returns(new MessageToReturnDto());


            var controllerMock = new MessagesController(repositoryMock.Object, mapperMock.Object);
            controllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = user
                }
            };
            var result = await controllerMock.CreateMessage(1, messageForCreation);
            Assert.IsType<CreatedAtRouteResult>(result);
        }
        [Fact]
        public async Task CreateMessage_InvalidRequest_ReturnsUnauthorizedResult()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
   {
                    new Claim(ClaimTypes.NameIdentifier, "1"),
        }));
            var messageForCreation = new MessageForCreationDto()
            {

                SenderId = 1,
                RecipientId = 1
            };
            var mapperMock = new Mock<IMapper>();
            var repositoryMock = new Mock<IRepositoryWrapper>();
            repositoryMock.Setup(r => r.UserRepository.GetUserAsync(1)).ReturnsAsync(new User
            {
                Id = 1
            });
            repositoryMock.Setup(r => r.UserRepository.GetUserAsync(2)).ReturnsAsync(new User
            {
                Id = 2
            });
            repositoryMock.Setup(r => r.SaveAllAsync()).ReturnsAsync(true);
            repositoryMock.Setup(r => r.MessageRepository.Add(It.IsAny<Message>()));
            mapperMock.Setup(m => m.Map<Message>(It.IsAny<MessageForCreationDto>())).Returns(new Message());
            mapperMock.Setup(m => m.Map<MessageToReturnDto>(It.IsAny<Message>())).Returns(new MessageToReturnDto());


            var controllerMock = new MessagesController(repositoryMock.Object, mapperMock.Object);
            controllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = user
                }
            };
            var result = await controllerMock.CreateMessage(2, messageForCreation);
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task DeleteMessage_ValidUserIdAndMessageIdPassed_ReturnsNoContentResult()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
{
                    new Claim(ClaimTypes.NameIdentifier, "1"),
}));
            var mapperMock = new Mock<IMapper>();
            var repositoryMock = new Mock<IRepositoryWrapper>();
            repositoryMock.Setup(r => r.MessageRepository.GetMessageAsync(It.IsAny<int>())).ReturnsAsync(new Message());
            repositoryMock.Setup(r => r.SaveAllAsync()).ReturnsAsync(true);

            var controllerMock = new MessagesController(repositoryMock.Object, mapperMock.Object);
            controllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = user
                }
            };
            var result = await controllerMock.DeleteMessage(1, 1);
            Assert.IsType<NoContentResult>(result);
        }
        [Fact]
        public async Task DeleteMessage_NotAuthorizedUserIdPassed_ReturnsUnauthorizedResult()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
{
                    new Claim(ClaimTypes.NameIdentifier, "1"),
}));
            var mapperMock = new Mock<IMapper>();
            var repositoryMock = new Mock<IRepositoryWrapper>();
            repositoryMock.Setup(r => r.MessageRepository.GetMessageAsync(It.IsAny<int>())).ReturnsAsync(new Message());
            repositoryMock.Setup(r => r.SaveAllAsync()).ReturnsAsync(true);

            var controllerMock = new MessagesController(repositoryMock.Object, mapperMock.Object);
            controllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = user
                }
            };
            var result = await controllerMock.DeleteMessage(2, 2);
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task MarkMessageAsRead_ValidUserIdAndMessageIdPassed_ReturnsNoContentResult()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
{
                    new Claim(ClaimTypes.NameIdentifier, "1"),
}));
            var mapperMock = new Mock<IMapper>();
            var repositoryMock = new Mock<IRepositoryWrapper>();
            repositoryMock.Setup(r => r.MessageRepository.GetMessageAsync(It.IsAny<int>())).ReturnsAsync(new Message()
            {
                RecipientId = 1
            });
            repositoryMock.Setup(r => r.SaveAllAsync()).ReturnsAsync(true);

            var controllerMock = new MessagesController(repositoryMock.Object, mapperMock.Object);
            controllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = user
                }
            };
            var result = await controllerMock.MarkMessageAsRead(1, 1);
            Assert.IsType<NoContentResult>(result);
        }
        [Fact]
        public async Task MarkMessageAsRead_UnauthorizedUserIdPassed_ReturnsUnauthorizedResult()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
{
                    new Claim(ClaimTypes.NameIdentifier, "1"),
}));
            var mapperMock = new Mock<IMapper>();
            var repositoryMock = new Mock<IRepositoryWrapper>();
            repositoryMock.Setup(r => r.MessageRepository.GetMessageAsync(It.IsAny<int>())).ReturnsAsync(new Message()
            {
                RecipientId = 1
            });
            repositoryMock.Setup(r => r.SaveAllAsync()).ReturnsAsync(true);

            var controllerMock = new MessagesController(repositoryMock.Object, mapperMock.Object);
            controllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = user
                }
            };
            var result = await controllerMock.MarkMessageAsRead(2, 2);
            Assert.IsType<UnauthorizedResult>(result);
        }
    }

}