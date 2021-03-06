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
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace CourseApp.Tests.BusinessLogic
{
    public class PhotosControllerTest
    {
        [Fact]
        public async Task GetPhoto_ValidIdPassed_ReturnsOkResult()
        {
            var photo = new Photo()
            {
                Id = 1
            };
            var photoToReturn = new PhotoForReturnDto()
            {
                Id = 1
            };
            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<PhotoForReturnDto>(photo)).Returns(photoToReturn);

            var cloudinaryConfigMock = Options.Create<CloudinarySettings>(new CloudinarySettings
            {
                ApiKey = "test",
                ApiSecret = "test",
                CloudName = "test"
            });

            var repositoryMock = new Mock<IRepositoryWrapper>();
            repositoryMock.Setup(r => r.PhotoRepository.GetPhotoAsync(It.IsAny<int>())).ReturnsAsync(photo);

            var controllerMock = new PhotosController(repositoryMock.Object, mapperMock.Object, cloudinaryConfigMock);

            var result = await controllerMock.GetPhoto(1);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetPhoto_InvalidIdPassed_ReturnsOkResult()
        {
            var photo = new Photo()
            {
                Id = 1
            };
            var photoToReturn = new PhotoForReturnDto()
            {
                Id = 1
            };
            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<PhotoForReturnDto>(photo)).Returns(photoToReturn);

            var cloudinaryConfigMock = Options.Create<CloudinarySettings>(new CloudinarySettings
            {
                ApiKey = "test",
                ApiSecret = "test",
                CloudName = "test"
            });

            var repositoryMock = new Mock<IRepositoryWrapper>();
            repositoryMock.Setup(r => r.PhotoRepository.GetPhotoAsync(It.IsAny<int>())).ReturnsAsync(photo);

            var controllerMock = new PhotosController(repositoryMock.Object, mapperMock.Object, cloudinaryConfigMock);

            var result = await controllerMock.GetPhoto(2);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task SetMainPhoto_ValidIdAndMainPhotoPassed_ReturnsBadRequest()
        {
            var photo = new Photo()
            {
                Id = 1,
                IsMain = true,
            };
            var user = new User()
            {
                Id = 1,
                Photos = new List<Photo> { photo }

            };
            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.NameIdentifier, "1"),
                  }));

            var mapperMock = new Mock<IMapper>();
            var cloudinaryConfigMock = Options.Create<CloudinarySettings>(new CloudinarySettings
            {
                ApiKey = "test",
                ApiSecret = "test",
                CloudName = "test"
            });

            var repositoryMock = new Mock<IRepositoryWrapper>();
            repositoryMock.Setup(r => r.UserRepository.GetUserAsync(It.IsAny<int>())).ReturnsAsync(() => user);
            repositoryMock.Setup(r => r.PhotoRepository.GetPhotoAsync(It.IsAny<int>())).ReturnsAsync(() => photo);
            repositoryMock.Setup(r => r.PhotoRepository.GetMainPhotoForUserAsync(It.IsAny<int>())).ReturnsAsync(() => photo);
            repositoryMock.Setup(r => r.SaveAllAsync()).ReturnsAsync(true);
            var controllerMock = new PhotosController(repositoryMock.Object, mapperMock.Object, cloudinaryConfigMock);


            controllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = userClaims
                }
            };

            var result = await controllerMock.SetMainPhoto(user.Id, photo.Id);

            Assert.IsType<BadRequestObjectResult>(result);

        }



        [Fact]
        public async Task SetMainPhoto_NotMainPhotoPassed_ReturnsBadRequest()
        {
            var photo = new Photo()
            {
                Id = 1,
                IsMain = true,
            };
            var photoToMain = new Photo()
            {
                Id = 2,
                IsMain = false,
            };
            var user = new User()
            {
                Id = 1,
                Photos = new List<Photo> { photo, photoToMain }

            };
            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.NameIdentifier, "1"),
                  }));

            var mapperMock = new Mock<IMapper>();
            var cloudinaryConfigMock = Options.Create<CloudinarySettings>(new CloudinarySettings
            {
                ApiKey = "test",
                ApiSecret = "test",
                CloudName = "test"
            });

            var repositoryMock = new Mock<IRepositoryWrapper>();
            repositoryMock.Setup(r => r.UserRepository.GetUserAsync(It.IsAny<int>())).ReturnsAsync(() => user);
            repositoryMock.Setup(r => r.PhotoRepository.GetPhotoAsync(It.IsAny<int>())).ReturnsAsync(() => photoToMain);
            repositoryMock.Setup(r => r.PhotoRepository.GetMainPhotoForUserAsync(It.IsAny<int>())).ReturnsAsync(() => photo);
            repositoryMock.Setup(r => r.SaveAllAsync()).ReturnsAsync(true);
            var controllerMock = new PhotosController(repositoryMock.Object, mapperMock.Object, cloudinaryConfigMock);


            controllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = userClaims
                }
            };

            var result = await controllerMock.SetMainPhoto(user.Id, photoToMain.Id);

            Assert.IsType<NoContentResult>(result);
            Assert.Equal(true, photoToMain.IsMain);
            Assert.Equal(false, photo.IsMain);

        }

        [Fact]
        public async Task AddPhotoForUser_UnauthorizedUserIdPassed_ReturnsUnauthorizedResult()
        {

            var photo = new Photo()
            {
                Id = 1,
                IsMain = true,
            };
            var photoToMain = new Photo()
            {
                Id = 2,
                IsMain = false,
            };
            var user = new User()
            {
                Id = 2,
                Photos = new List<Photo> { photo, photoToMain }

            };
            var photoForCreation = new PhotoForCreationDto
            {
                File = new FormFile(null, 0, 0, "", ""),
                Url = "test",
                PublicId = "test"
            };
            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                   {
                    new Claim(ClaimTypes.NameIdentifier, "1"),
                 }));

            var mapperMock = new Mock<IMapper>();
            var cloudinaryConfigMock = Options.Create<CloudinarySettings>(new CloudinarySettings
            {
                ApiKey = "test",
                ApiSecret = "test",
                CloudName = "test"
            });
            var repositoryMock = new Mock<IRepositoryWrapper>();
            repositoryMock.Setup(r => r.UserRepository.GetUserAsync(It.IsAny<int>())).ReturnsAsync(() => user);
            repositoryMock.Setup(r => r.SaveAllAsync()).ReturnsAsync(true);
            mapperMock.Setup(m => m.Map<PhotoForReturnDto>(It.IsAny<Photo>())).Returns(new PhotoForReturnDto());
            mapperMock.Setup(m => m.Map<Photo>(It.IsAny<PhotoForCreationDto>())).Returns(new Photo());

            var controllerMock = new PhotosController(repositoryMock.Object, mapperMock.Object, cloudinaryConfigMock);


            controllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = userClaims
                }
            };


            var result = await controllerMock.AddPhotoForUser(user.Id, photoForCreation);
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task DeletePhoto_ValidUserIdAndPhotoIdPassed_ReturnsOkResult()
        {
            var photo = new Photo()
            {
                Id = 1,
                IsMain = true,
            };
            var photoToDelete = new Photo()
            {
                Id = 2,
                IsMain = false,
            };
            var user = new User()
            {
                Id = 1,
                Photos = new List<Photo> { photo, photoToDelete }

            };
            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                   {
                    new Claim(ClaimTypes.NameIdentifier, "1"),
                 }));

            var mapperMock = new Mock<IMapper>();
            var cloudinaryConfigMock = Options.Create<CloudinarySettings>(new CloudinarySettings
            {
                ApiKey = "test",
                ApiSecret = "test",
                CloudName = "test"
            });
            var repositoryMock = new Mock<IRepositoryWrapper>();
            repositoryMock.Setup(r => r.UserRepository.GetUserAsync(It.IsAny<int>())).ReturnsAsync(() => user);
            repositoryMock.Setup(r => r.PhotoRepository.GetPhotoAsync(It.IsAny<int>())).ReturnsAsync(() => photoToDelete);
            repositoryMock.Setup(r => r.PhotoRepository.Delete(It.IsAny<Photo>()));
            repositoryMock.Setup(r => r.SaveAllAsync()).ReturnsAsync(true);

            var controllerMock = new PhotosController(repositoryMock.Object, mapperMock.Object, cloudinaryConfigMock);


            controllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = userClaims
                }
            };


            var result = await controllerMock.DeletePhoto(user.Id, 2);
            Assert.IsType<OkResult>(result);

        }

        [Fact]
        public async Task DeletePhoto_UnauthorizedUserIdPassed_ReturnsUnauthorizedResult()
        {
            var photo = new Photo()
            {
                Id = 1,
                IsMain = true,
            };
            var photoToDelete = new Photo()
            {
                Id = 2,
                IsMain = false,
            };
            var user = new User()
            {
                Id = 3,
                Photos = new List<Photo> { photo, photoToDelete }

            };
            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                   {
                    new Claim(ClaimTypes.NameIdentifier, "1"),
                 }));

            var mapperMock = new Mock<IMapper>();
            var cloudinaryConfigMock = Options.Create<CloudinarySettings>(new CloudinarySettings
            {
                ApiKey = "test",
                ApiSecret = "test",
                CloudName = "test"
            });
            var repositoryMock = new Mock<IRepositoryWrapper>();
            repositoryMock.Setup(r => r.UserRepository.GetUserAsync(It.IsAny<int>())).ReturnsAsync(() => user);
            repositoryMock.Setup(r => r.PhotoRepository.GetPhotoAsync(It.IsAny<int>())).ReturnsAsync(() => photoToDelete);
            repositoryMock.Setup(r => r.PhotoRepository.Delete(It.IsAny<Photo>()));
            repositoryMock.Setup(r => r.SaveAllAsync()).ReturnsAsync(true);

            var controllerMock = new PhotosController(repositoryMock.Object, mapperMock.Object, cloudinaryConfigMock);


            controllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = userClaims
                }
            };


            var result = await controllerMock.DeletePhoto(user.Id, 2);
            Assert.IsType<UnauthorizedResult>(result);

        }

        [Fact]
        public async Task DeletePhoto_MainPhotoPassed_ReturnsBadRequestResult()
        {
            var photo = new Photo()
            {
                Id = 1,
                IsMain = true,
            };
            var photoToDelete = new Photo()
            {
                Id = 2,
                IsMain = false,
            };
            var user = new User()
            {
                Id = 1,
                Photos = new List<Photo> { photo, photoToDelete }

            };
            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                   {
                    new Claim(ClaimTypes.NameIdentifier, "1"),
                 }));

            var mapperMock = new Mock<IMapper>();
            var cloudinaryConfigMock = Options.Create<CloudinarySettings>(new CloudinarySettings
            {
                ApiKey = "test",
                ApiSecret = "test",
                CloudName = "test"
            });
            var repositoryMock = new Mock<IRepositoryWrapper>();
            repositoryMock.Setup(r => r.UserRepository.GetUserAsync(It.IsAny<int>())).ReturnsAsync(() => user);
            repositoryMock.Setup(r => r.PhotoRepository.GetPhotoAsync(It.IsAny<int>())).ReturnsAsync(() => photo);
            repositoryMock.Setup(r => r.PhotoRepository.Delete(It.IsAny<Photo>()));
            repositoryMock.Setup(r => r.SaveAllAsync()).ReturnsAsync(true);

            var controllerMock = new PhotosController(repositoryMock.Object, mapperMock.Object, cloudinaryConfigMock);


            controllerMock.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = userClaims
                }
            };


            var result = await controllerMock.DeletePhoto(user.Id, 1);
            Assert.IsType<BadRequestObjectResult>(result);

        }




    }

}