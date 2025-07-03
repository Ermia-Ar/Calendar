using Core.Domain.Entities.Requests;
using Core.Domain.Enum;
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

	Task<IResponse?> GetById(string id, CancellationToken token);

	Task<ListDto> GetAllProjects(string userId, GetAllProjectsFiltering filtering 
		, GetAllProjectsOrdring order, PaginationFilter pagination
		, CancellationToken token);

	Task<ListDto> GetProjectActivies(string userId, string projectId
		, GetProjectActivitiesFiltering filtering
		, GetProjectActivitiesOrdering ordering, PaginationFilter pagination
		, CancellationToken token);


	Task<ListDto> GetAllActivities(string userId, string projectId
		, GetAllActivitiesFiltering filtering
		, GetAllActivitiesOrdering ordering, PaginationFilter pagination
		, CancellationToken token);


	Task<IReadOnlyCollection<IResponse>> GetMemberOfProject(string projectId, CancellationToken token);

	Task<IReadOnlyCollection<IResponse>> GetMemberOfActivity(string activityId, CancellationToken token);



	Task<string[]> FindMemberIdsOfProject(string projectId, CancellationToken token);

	Task<string[]> FindMemberIdsOfActivity(string activityId, CancellationToken token);

	Task<Request?> FindById(string id, CancellationToken token);

	Task<IReadOnlyCollection<Request>> Find(string? projectId
		  , string? activityId, string? receiverId, string? senderId
		  , RequestStatus? status, CancellationToken token, bool? isGeust = null);

	Task<Request?> FindOne(string activityId, string receiverId, CancellationToken token);

	Task<string[]> FindProjectIds(string userId, CancellationToken token);

	Task<string[]> FindActivityIds(string userId, CancellationToken token);

}
