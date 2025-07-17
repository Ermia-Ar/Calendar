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
	void Remove(ActivityRequest activityRequest);
	void RemoveRange(ICollection<ActivityRequest> requests);
	void Add(ActivityRequest activityRequest);
	void AddRange(ICollection<ActivityRequest> requests);
	void Update(ActivityRequest activityRequest);

	//Queries
	Task<ListDto> GetAll(GetAllRequestFiltering filtering
		, GetAllRequestsOrdring order, PaginationFilter pagination
		, CancellationToken token);

	Task<IResponse?> GetById(long id, CancellationToken token);

	Task<ActivityRequest?> FindById(long id, CancellationToken token);

}
