using AutoMapper;
using Core.Application.DTOs.ActivityDTOs;
using Core.Application.DTOs.AuthDTOs;
using Core.Application.DTOs.UserDTOs;
using Core.Application.DTOs.UserRequestDTOs;
using Infrastructure.Base.Utility;
using Infrastructure.Entities;
using Infrastructure.Entity;

namespace Infrastructure.Base.Mapper
{
    public class InfraProfile : Profile
    {
        public InfraProfile()
        {
            // user mappers
            CreateMap<RegisterRequest, User>()
                .ReverseMap();

            //activity mapper 
            CreateMap<Activity, CreateActivityRequest>()
                .ReverseMap();

            CreateMap<Activity, ActivityResponse>()
                //.ForMember(x => x.DurationInMinute , dex => dex.MapFrom(x => Convert.ToDouble(,)))
                .ReverseMap();

            //request mapper
            //CreateMap<UserRequest, UserRequestResponse>()
            //    .ForMember(x => x.Activity, dex => dex.MapFrom(x => Utilities.ConvertToActivityResponse(x.Activity)))
            //    .ReverseMap();

            CreateMap<SendRequest, UserRequest>()
                .ReverseMap();

            //user mapper
            CreateMap<User , UserResponse>()
                .ReverseMap();

        }
    }
}
