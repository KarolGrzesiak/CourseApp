using System.Linq;
using AutoMapper;
using CourseApp.API.Dtos;
using CourseApp.API.Model;

namespace CourseApp.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<UserForRegisterDto, User>();
            CreateMap<User, UserForListDto>().ForMember(dest => dest.PhotoUrl, opt =>
          {
              opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url);
          }).ForMember(dest => dest.Age, opt =>
          {
              opt.ResolveUsing(src => src.DateOfBirth.CalculateAge());
          });
            CreateMap<UserForUpdateDto, User>();
        }

    }
}