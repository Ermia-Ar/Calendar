using Core.Application.ApplicationServices.Auth.Queries.GetAll;
using Core.Application.ApplicationServices.Auth.Queries.GetById;
using Core.Domain.Entity.Users;
using Core.Domain.Enum;
using Core.Domain.Interfaces.Repositories;
using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SharedKernel.Helper;
using System.Collections.Immutable;


namespace Infrastructure.Repositories
{
    public class UserRepository : IUsersRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;

        //commands
        public UserRepository(UserManager<User> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<string[]?> AddRoleToUser(User user, string roleName)
        {
            var errors = (await _userManager.AddToRoleAsync
                (user, roleName)).Errors;
            return errors.Select(x => x.ToString()).ToArray();
        }

        public async Task<string[]?> AddUser(User user, string password)
        {
            var errors = (await _userManager.CreateAsync
                (user, password)).Errors;
            return errors.Select(x => x.ToString()).ToArray();
        }

        //Queries
        public async Task<bool> CheckPasswordAsync(User user, string password) =>
            await _userManager.CheckPasswordAsync(user, password);

        public async Task<User?> FindByEmail(string email) =>
           (await _userManager.FindByEmailAsync(email));

        public async Task<User?> FindByUserName(string userName) =>
           (await _userManager.FindByNameAsync(userName));

        public async Task<User?> FindById(string userId) =>
            (await _userManager.FindByIdAsync(userId));

        //public async Task<List<User>> GetAll(string? search , UserCategory? category, CancellationToken token)
        //{
        //    var users = _userManager.Users;
        //    if(search != null)
        //    {
        //        users = users.Where(x => x.UserName.Contains(search));
        //    }
        //    if (category.HasValue)
        //    {
        //        users = users.Where(x => x.Category == category);
        //    }
        //    return (await users.ToListAsync(token));
        //}

        public async Task<IReadOnlyCollection<IResponse>> GetAll(string? search, UserCategory? category, CancellationToken token)
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("Connection"));
            await connection.OpenAsync();

            var parameters = new DynamicParameters();
            parameters.Add("search", search);
            parameters.Add("category", category);

            var result = await connection
                .QueryAsync<GetAllUserQueryResponse>("SP_GetAllUsers", parameters, commandType: System.Data.CommandType.StoredProcedure);

            return result.ToImmutableList();
        }


        public async Task<IResponse?> GetById(string id, CancellationToken token)
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("Connection"));
            await connection.OpenAsync();

            var parameters = new DynamicParameters();
            parameters.Add("id", id);
            parameters.Add("userName", null);

            var result = await connection
                .QueryAsync<GetUserByIdQueryResponse>("SP_GetUserBy", parameters, commandType: System.Data.CommandType.StoredProcedure);

            return result.SingleOrDefault();
        }

        public async Task<IResponse?> GetByUserName(string userName, CancellationToken token)
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("Connection"));
            await connection.OpenAsync();

            var parameters = new DynamicParameters();
            parameters.Add("id", null);
            parameters.Add("userName", userName);

            var result = await connection
                .QueryAsync<GetAllUserQueryResponse>("SP_GetAllUsers", parameters, commandType: System.Data.CommandType.StoredProcedure);
            return result.SingleOrDefault();
        }
    }
}
