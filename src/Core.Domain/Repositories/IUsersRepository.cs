using Core.Domain.Filtering;
using Core.Domain.Odering;
using SharedKernel.Dtos;
using SharedKernel.Helper;
using SharedKernel.QueryFilterings;

namespace Core.Domain.Repositories;

public interface IUsersRepository
{
    Task<ListDto> GetAll(GetAllUsersFiltering filtering
        , GetAllUsersOrdering order, PaginationFilter pagination
        , CancellationToken token);
    
    Task<IResponse?> GetById(Guid id, CancellationToken token = default);
}