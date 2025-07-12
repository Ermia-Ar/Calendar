using Core.Domain.Entities.ProjectMembers;
using Core.Domain.Filtering;
using Core.Domain.Odering;
using SharedKernel.Dtos;
using SharedKernel.QueryFilterings;

namespace Core.Domain.Repositories;

public interface IProjectMembersRepository
{
	//Commnads
	void Remove(ProjectMember projectMember);

	Task<ProjectMember?> FindById(long id,
		CancellationToken token);

	void RemoveRange(ICollection<ProjectMember> projectMembers);

	ProjectMember Add(ProjectMember projectMember);

	void AddRange(ICollection<ProjectMember> projectMember);

	//Queries
	Task<bool> IsMemberOfProject(long projectId,
		Guid userId, CancellationToken token);

	Task<IReadOnlyCollection<Guid>> FindMemberIdsOfProject
		(long projectId, CancellationToken token);

	Task<ProjectMember?> GetByUserIdAndProjectId(Guid userId, long projectId
		, CancellationToken token);

	Task<IReadOnlyCollection<ProjectMember>> FindByProjectId(long projectId, CancellationToken token);

	Task<ListDto> GetProjectOfUserId(
		Guid userId, GetAllProjectsFiltering filtering,
		GetAllProjectsOrdring ordering, PaginationFilter pagination,
		CancellationToken token);

	Task<ListDto> GetMemberOfProject(long projectId,
		PaginationFilter pagination, CancellationToken token);
}
