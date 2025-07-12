using Core.Domain.Entities.ActivityMembers;
using Core.Domain.Filtering;
using Core.Domain.Odering;
using SharedKernel.Dtos;
using SharedKernel.QueryFilterings;

namespace Core.Domain.Repositories;

public interface IActivityMembersRepository
{
	//Commands
	void Remove(ActivityMember activityMember);

	Task<ActivityMember?> FindById(long id, CancellationToken token);

	void RemoveRange(ICollection<ActivityMember> activityMembers);

	ActivityMember Add(ActivityMember activityMember);

	void AddRange(ICollection<ActivityMember> activityMembers);

	//Queries
	Task<bool> IsMemberOfActivity(Guid userId,long activityId,
		CancellationToken token);

	Task<IReadOnlyCollection<long>> FindActivityMemberIdsOfActivity
		(long activityId, CancellationToken token);	
	
	Task<IReadOnlyCollection<ActivityMember>> FindByActivityId
		(long activityId, CancellationToken token);

	Task<ListDto> GetActivitiesForUserId(Guid userId, long projectId,
		GetAllActivitiesFiltering filtering,
		GetAllActivitiesOrdering ordering,
		PaginationFilter pagination,
		CancellationToken token);

	Task<IReadOnlyCollection<Guid>> FindMemberIdsOfActivity(long activityId, 
		CancellationToken token);
	
	Task<ListDto> GetMemberOfActivity(long activityId, PaginationFilter pagination,
		CancellationToken token);

	Task<ActivityMember?> FindByActivityIdAndMemberId(Guid userId, long activityId,
		CancellationToken token);

	Task<ListDto> GetActivitiesOfProjectForUserId(Guid userId, long projectId,
		GetProjectActivitiesFiltering filtering, 
		GetProjectActivitiesOrdering ordering, 
		PaginationFilter pagination,
		 CancellationToken token);

	Task<IReadOnlyCollection<ActivityMember>> FindActivityMemberOfActivitiesForProjectForUserId(Guid userId, long projectId,
		 CancellationToken token);
}

