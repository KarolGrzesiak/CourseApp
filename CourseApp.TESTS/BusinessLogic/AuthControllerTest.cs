using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CourseApp.API;
using CourseApp.API.Controllers;
using CourseApp.API.Dtos;
using CourseApp.API.Model;
using CourseApp.Tests.FakeModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace CourseApp.Tests.BusinessLogic
{
    public class AuthControllerTest
    {
        [Fact]
        public async Task Register_ValidUserPassed_ReturnsCreatedAtRoute()
        {

            var userForRegister = new UserForRegisterDto
            {
                UserName = "test",
                Password = "password"
            };
            var user = new User
            {
                UserName = "test"
            };
            var role = "Student";


            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<User>(It.IsAny<UserForRegisterDto>())).Returns(() => user);

            var configurationMock = new Mock<IConfiguration>();

            var listOfCreatedUsers = new List<User>();
            var userManagerMock = new Mock<FakeUserManager>();
            var signInManagerMock = new Mock<FakeSignInManager>();
            userManagerMock.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success).Callback<User, string>((x, y) =>
            {
                user.PasswordHash = userForRegister.Password;
                listOfCreatedUsers.Add(x);
            }
                );
            userManagerMock.Setup(x => x.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success).Callback<User, string>((x, y) => role = y);

            var controllerMock = new AuthController(configurationMock.Object, mapperMock.Object, userManagerMock.Object, signInManagerMock.Object);

            var result = await controllerMock.Register(userForRegister);

            Assert.IsType<CreatedAtRouteResult>(result);
            Assert.Equal(1, listOfCreatedUsers.Count);
            Assert.Equal("password", user.PasswordHash);




        }


        [Fact]
        public async Task Register_NotValidUserPassed_ReturnsBadRequest()
        {

            var userForRegister = new UserForRegisterDto
            {
                UserName = "",
                Password = ""
            };
            var user = new User
            {
                UserName = ""
            };


            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<User>(It.IsAny<UserForRegisterDto>())).Returns(() => user);
            var configurationMock = new Mock<IConfiguration>();
            var userManagerMock = new Mock<FakeUserManager>();
            var signInManagerMock = new Mock<FakeSignInManager>();
            userManagerMock.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed(new IdentityError()));

            var controllerMock = new AuthController(configurationMock.Object, mapperMock.Object, userManagerMock.Object, signInManagerMock.Object);

            var result = await controllerMock.Register(userForRegister);
            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact]
        public bool Login_ValidRequest_ReturnsOkResult()
        {
            var userForLogin = new UserForLoginDto
            {
                UserName = "test",
                Password = "test"
            };

            var user = new User()
            {
                UserName = "test",
                PasswordHash = "test"
            };
            var userForList = new UserForListDto()
            {
                Username = "test"
            };
            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<UserForListDto>(It.IsAny<User>())).Returns(() => userForList);
            var configurationMock = new Mock<IConfiguration>();
            var userManagerMock = new Mock<FakeUserManager>();
            var signInManagerMock = new Mock<FakeSignInManager>();

            var controllerMock = new AuthController(configurationMock.Object, mapperMock.Object, userManagerMock.Object, signInManagerMock.Object);
            return true;

        }





    }
}