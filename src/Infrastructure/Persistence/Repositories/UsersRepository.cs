namespace Infrastructure.Persistence.Repositories;

public class UsersRepository : IUsersRepository
{
    public Task<ListDto> GetAll(GetAllUsersFiltering filtering, 
        GetAllUsersOrdering order, PaginationFilter pagination,
        CancellationToken token)
    {
        throw new NotImplementedException();
    }

    public Task<IResponse?> GetById(Guid id, CancellationToken token)
    {
        throw new NotImplementedException();
    }
}