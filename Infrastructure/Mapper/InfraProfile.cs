using DomainUser = Core.Domain.Entity.User;
using AutoMapper;
using Infrastructure.Models;

namespace Infrastructure.Mapper
{
    public class InfraProfile : Profile
    {
        public InfraProfile()
        {
            CreateMap<User, DomainUser>()
                .ReverseMap();
        }
    }
}
