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

            CreateMap<User, UserForListDto>().ForMember(dest => dest.PhotoUrl, opt =>
          {
              opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url);
          }).ForMember(dest => dest.Age, opt =>
          {
              opt.ResolveUsing(src => src.DateOfBirth.CalculateAge());
          });
            CreateMap<User, UserForDetailedDto>()
            .ForMember(dest => dest.PhotoUrl, opt =>
          {
              opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url);
          }).ForMember(dest => dest.Age, opt =>
          {
              opt.ResolveUsing(src => src.DateOfBirth.CalculateAge());
          }).ForMember(dest => dest.Roles, opt =>
          {
              opt.MapFrom(src => src.UserRoles.Select(ur => ur.Role.Name).ToList());
          });
            CreateMap<UserForRegisterDto, User>();
            CreateMap<UserForUpdateDto, User>();
            CreateMap<Photo, PhotosForDetailedDto>();
            CreateMap<Photo, PhotoForReturnDto>();
            CreateMap<PhotoForCreationDto, Photo>();
            CreateMap<MessageForCreationDto, Message>().ReverseMap();
            CreateMap<Message, MessageToReturnDto>()
                .ForMember(dest => dest.SenderPhotoUrl, opt => opt.MapFrom(src => src.Sender.Photos.FirstOrDefault(p => p.IsMain).Url))
                .ForMember(dest => dest.RecipientPhotoUrl, opt => opt.MapFrom(src => src.Recipient.Photos.FirstOrDefault(p => p.IsMain).Url));
            CreateMap<ExamForCreationDto, Exam>();
            CreateMap<Exam, ExamForListDto>()
                .ForMember(dest => dest.AuthorKnownAs, opt => opt.MapFrom(src => src.Author.KnownAs));
            CreateMap<Question, QuestionForReturnDto>();
            CreateMap<QuestionForCreationDto, Question>();
            CreateMap<AnswerForCreationDto, Answer>();
        }


    }
}