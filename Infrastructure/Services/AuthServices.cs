using AutoMapper;
using Core.Application.DTOs.AuthDTOs;
using Core.Application.DTOs.UserDTOs;
using Core.Application.Interfaces;
using Core.Domain.Shared;
using Infrastructure.Base.Utility;
using Infrastructure.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Infrastructure.Services
{
    public class AuthServices : IAuthServices
    {
        private UserManager<User> _userManager;
        private IMapper _mapper;

        public AuthServices(UserManager<User> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<Result<List<UserResponse>>> GetAllUsers()
        {
            try
            {
                var users = await _userManager.Users.ToListAsync();
                var userResponse = _mapper.Map<List<UserResponse>>(users);

                return Result.Success(userResponse);
            }
            catch
            {
                return Result.Failure<List<UserResponse>>(new Error("", "something wrong !"));
            }
        }

        public async Task<Result<UserResponse>> GetUserByUserName(string userName)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(userName);
                var userResponse = _mapper.Map<UserResponse>(user);
                return userResponse;
            }
            catch
            {
                return Result.Failure<UserResponse>(Error.None);
            }
        }

        public async Task<Result> Login(LoginRequest request)
        {
            User user;
            //check email or user name
            if (Utilities.IsEmail(request.UserNameOrEmail))
            {
                user = await _userManager.FindByEmailAsync(request.UserNameOrEmail);
            }
            else
            {
                user = await _userManager.FindByNameAsync(request.UserNameOrEmail);
            }
            if (user == null)
            {
                return Result.Failure(new Error("", "UserName or Password is wrong"));
            }
            // check password 
            bool checkPassword = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!checkPassword)
            {
                return Result.Failure(new Error("", "UserName or Password is wrong"));
            }

            return Result.Success();
        }

        public async Task<Result> Register(RegisterRequest request)
        {
            //map to User
            var user = _mapper.Map<User>(request);

            //Add to user table
            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                return Result.Failure(new Error("", result.Errors.First().Description));
            }
            result = await _userManager.AddToRoleAsync(user, "User");
            if (!result.Succeeded)
            {
                return Result.Failure(new Error("", result.Errors.First().Description));
            }

            return Result.Success();
        }
    }
}
