using Core.Application.DTOs.AuthDTOs;
using Core.Application.DTOs.UserDTOs;
using Core.Application.Features.Auth.Commands;
using Core.Application.Features.Auth.Queries;
using Core.Domain.Enum;
using Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml.FormulaParsing.FormulaExpressions;
using SharedKernel.ResponseResult;

namespace Calendar.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<SuccessResponse<string>> Register([FromBody] RegisterRequest registerRequest)
        {
            var request = new RegisterCommand(registerRequest);
            var result = await _mediator.Send(request);

            return Result.Ok(result);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<SuccessResponse<string>> Login([FromBody] LoginRequest loginRequest)
        {
            var request = new LoginCommand (loginRequest);
            var result = await _mediator.Send(request);

            return Result.Ok(result);
        }

        [HttpGet]
        [Route("GetUserByUserName")]
        //[Authorize(CalendarClaims.GetUserByUserName)]
        public async Task<SuccessResponse<UserResponse>> GetUserByUserName(string userName)
        {
            var request = new GetUserByUserNameQuery (userName);
            var result = await _mediator.Send(request);

            return Result.Ok(result);
        }

        [HttpGet]
        [Route("GetAllUsers")]
        //[Authorize(CalendarClaims.GetAllUsers)]
        public async Task<SuccessResponse<List<UserResponse>>> GetAllUsers(string? search , UserCategory? category)
        {
            var request = new GetAllUsersQuery(search, category);
            var result = await _mediator.Send(request);

            return Result.Ok(result);
        }
    }
}
