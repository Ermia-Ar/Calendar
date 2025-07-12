using Core.Domain.Entities.Requests;
using Core.Domain.Filtering;
using Core.Domain.Odering;
using SharedKernel.Dtos;
using SharedKernel.Helper;
using SharedKernel.QueryFilterings;

namespace Core.Domain.Repositories;

public interface IRequestsRepository
{
	//Commands
	void Remove(Request request);
	void RemoveRange(ICollection<Request> requests);
	void Add(Request request);
	void AddRange(ICollection<Request> requests);
	void Update(Request request);

	//Queries
	Task<ListDto> GetAll(GetAllRequestFiltering filtering
		, GetAllRequestsOrdring order, PaginationFilter pagination
		, CancellationToken token);

	Task<IResponse?> GetById(long id, CancellationToken token);

	Task<Request?> FindById(long id, CancellationToken token);

}
