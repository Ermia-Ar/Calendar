﻿using Calendar.Api.Base;
using Core.Application.DTOs.AuthDTOs;
using Core.Application.Features.Auth.Commands;
using Core.Application.Features.Auth.Queries;
using Core.Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Calendar.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : AppControllerBase
    {
        private IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            var request = new RegisterCommand { RegisterRequest = registerRequest };
            var result = await _mediator.Send(request);
            return NewResult(result);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var request = new LoginCommand { LoginRequest = loginRequest };
            var result = await _mediator.Send(request);
            return NewResult(result);
        }

        [HttpGet]
        [Route("GetUserByUserName")]
        //[Authorize(CalendarClaims.GetUserByUserName)]
        public async Task<IActionResult> GetUserByUserName(string userName)
        {
            var request = new GetUserByUserNameQuery { UserName = userName };
            var result = await _mediator.Send(request);
            return NewResult(result);
        }

        [HttpGet]
        [Route("GetAllUsers")]
        //[Authorize(CalendarClaims.GetAllUsers)]
        public async Task<IActionResult> GetAllUsers(string? search , UserCategory? category)
        {
            var request = new GetAllUsersQuery() { Category = category , Search = search};
            var result = await _mediator.Send(request);
            return NewResult(result);
        }
    }
}
