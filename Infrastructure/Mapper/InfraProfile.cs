using AutoMapper;
using DomainUser = Core.Domain.Entity.Users.User;
using User = Infrastructure.Models.User;

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
