using Core.Application.InternalServices.Auth.Dto;
using Core.Application.InternalServices.Auth.Dtos;

namespace Core.Application.InternalServices.Auth.Services;

public interface IUserSrevices
{
    Task<Responses<LoginRequestResponse>> Login(LoginRequestDto model);
}
