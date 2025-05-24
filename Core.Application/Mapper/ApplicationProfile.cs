using AutoMapper;
using Core.Application.DTOs.ActivityDTOs;
using Core.Application.DTOs.AuthDTOs;
using Core.Application.DTOs.ProjectDTOs;
using Core.Application.DTOs.UserDTOs;
using Core.Application.DTOs.UserRequestDTOs;
using Core.Application.Features.Comments.Queries.GetComments;
using Core.Application.Utility;
using Core.Domain.Entity;

namespace Core.Application.Mapper
{
    public class ApplicationProfile : Profile
    {
        public ApplicationProfile()
        {
            //comment mappers
            CreateMap<Comment , GetCommentsResponse>()
                .ReverseMap();
            
            //project mappers
            CreateMap<Project , CreateProjectRequest>()
                .ReverseMap();

            CreateMap<Project , ProjectResponse>()
                .ReverseMap();

            //activity mapper 
            CreateMap<Activity, CreateActivityRequest>()
                .ReverseMap();
           
            CreateMap<Activity, CreateActivityForProjectRequest>()
                .ReverseMap();
            
            CreateMap<Activity, CreateSubActivityRequest>()
                .ReverseMap();

            CreateMap<Activity, ActivityResponse>()
                .ReverseMap();

            CreateMap<Activity, UpdateActivityRequest>()
                .ReverseMap();


            //user requests Mapper
            CreateMap<SendActivityRequest, UserRequest>()
                .ReverseMap();

            CreateMap<UserRequest, RequestResponse>()
                //.ForMember(x => x.Activity, dex => dex.MapFrom(x => Utilities.ConvertToActivityResponse(x.Activity)))
                //.ForMember(x => x.Project, dex => dex.MapFrom(x => Utilities.ConvertToProjectResponse(x.Project)))
                .ReverseMap();

            CreateMap<SendProjectRequest, UserRequest>()
                .ReverseMap();

            //user mapper
            CreateMap<User, UserResponse>()
                .ReverseMap();

            CreateMap<RegisterRequest, User>()
                .ReverseMap();
        }
    }
}
