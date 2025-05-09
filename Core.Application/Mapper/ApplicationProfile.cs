using AutoMapper;
using Core.Application.DTOs.ActivityDTOs;
using Core.Application.DTOs.AuthDTOs;
using Core.Application.DTOs.ProjectDTOs;
using Core.Application.DTOs.UserDTOs;
using Core.Application.DTOs.UserRequestDTOs;
using Core.Domain.Entity;

namespace Core.Application.Mapper
{
    public class ApplicationProfile : Profile
    {
        public ApplicationProfile()
        {
            
            //project mappers
            CreateMap<Project , CreateProjectRequest>()
                .ReverseMap();

            CreateMap<Project , ProjectResponse>()
                .ReverseMap();

            //activity mapper 
            CreateMap<Activity, CreateActivityRequest>()
                .ReverseMap();

            CreateMap<Activity, ActivityResponse>()
                //.ForMember(x => x.DurationInMinute , dex => dex.MapFrom(x => Convert.ToDouble(,)))
                .ReverseMap();
            //user requests Mapper
            CreateMap<SendActivityRequest, UserRequest>()
                .ReverseMap();
            
            CreateMap<SendActivityRequest, UserRequest>()
                .ReverseMap();
            CreateMap<UserRequest, ActivityRequestResponse>()
                .ReverseMap();

            CreateMap<SendProjectRequest, UserRequest>()
                .ReverseMap();
            CreateMap<UserRequest, ProjectRequestResponse>()
                .ReverseMap();
            //user mapper
            CreateMap<User, UserResponse>()
                .ReverseMap();

            CreateMap<RegisterRequest, User>()
                .ReverseMap();
        }
    }
}
