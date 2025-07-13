using Core.Domain.Entities.Base;
using Core.Domain.Entities.Projects;

namespace Core.Domain.Entities.ProjectMembers;

public class ProjectMember : BaseEntity
{
	public Guid MemberId { get; set; }

	public long ProjectId { get; set; }
	public Project Project { get; set; }	

	public bool IsOwner { get; set; }

	public static ProjectMember Create(Guid memberId, long projectId)
	{
		return new ProjectMember
		{
			MemberId = memberId,
			ProjectId = projectId,
			CreatedDate = DateTime.UtcNow,
			IsActive = true,
			IsOwner = false
		};
	}
	
	public static ProjectMember CreateOwner(Guid memberId, long projectId)
	{
		return new ProjectMember
		{
			MemberId = memberId,
			ProjectId = projectId,
			CreatedDate = DateTime.UtcNow,
			IsActive = true,
			IsOwner = true,
		};
	}
}
