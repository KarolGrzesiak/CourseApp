using AutoMapper;
using CourseApp.API.Dtos;
using CourseApp.API.Model;

namespace CourseApp.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<UserForRegisterDto,User>();
        }

    }
}