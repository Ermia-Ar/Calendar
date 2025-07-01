

namespace Infrastructure.Persistance.Repositories
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
		   await _userManager.FindByEmailAsync(email);

		public async Task<User?> FindByUserName(string userName) =>
		   await _userManager.FindByNameAsync(userName);

		public async Task<User?> FindById(string userId) =>
			await _userManager.FindByIdAsync(userId);


		public async Task<ListDto> GetAll(GetAllUsersFiltering filtering
			, GetAllUsersOrdering ordering, PaginationFilter pagination
			, CancellationToken token)
		{
			using var connection = new SqlConnection(_configuration.GetConnectionString("Connection"));
			await connection.OpenAsync();

			var parameters = new DynamicParameters();

			//filtering
			parameters.Add("search", filtering.Search);
			parameters.Add("category", filtering.Category);
			//ordring
			parameters.Add("OrderDirection", ordering.GetOrderDirection(ordering));
			parameters.Add("OrderBy", ordering.GetOrderBy(ordering));
			//pagination
			parameters.Add("PageNumber", pagination.PageNumber);
			parameters.Add("PageSize", pagination.PageSize);

			parameters.Add("TotalCount", dbType: DbType.Int32, direction: ParameterDirection.Output);

			var result = await connection
				.QueryAsync<GetAllUserQueryResponse>("SP_GetAllUsers", parameters, commandType: CommandType.StoredProcedure);

			var totalCount = parameters.Get<int>("TotalCount");

			return new ListDto(totalCount, result.ToImmutableList());
		}


		public async Task<IResponse?> GetById(string id, CancellationToken token)
		{
			using var connection = new SqlConnection(_configuration.GetConnectionString("Connection"));
			await connection.OpenAsync();

			var parameters = new DynamicParameters();
			parameters.Add("id", id);
			parameters.Add("userName", null);

			var result = await connection
				.QueryAsync<GetUserByIdQueryResponse>("SP_GetUserBy", parameters, commandType: CommandType.StoredProcedure);

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
				.QueryAsync<GetAllUserQueryResponse>("SP_GetAllUsers", parameters, commandType: CommandType.StoredProcedure);
			return result.SingleOrDefault();
		}
	}
}
