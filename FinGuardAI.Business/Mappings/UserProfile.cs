using AutoMapper;
using FinGuardAI.DataAccess.DTOs;
using FinGuardAI.DataAccess.Entities;

namespace FinGuardAI.Business.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>()

               .ForMember(dest => dest.UserID, opt => opt.MapFrom(src => src.Id))
               .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.UserName))
               .ReverseMap()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserID))
               .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Username));



            CreateMap<UserAddDTO, User>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Username))
            .ReverseMap()
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.UserName));
        }


    }
}

