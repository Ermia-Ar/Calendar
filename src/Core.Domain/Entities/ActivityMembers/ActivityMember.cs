using Core.Domain.Entities.Activities;
using Core.Domain.Entities.Base;
using Core.Domain.Entities.Notifications;
using Core.Domain.Enum;
using Serilog.Sinks.SystemConsole.Themes;

namespace Core.Domain.Entities.ActivityMembers;

public class ActivityMember : BaseEntity
{
	public Guid MemberId { get; set; }

	public long ActivityId { get; set; }
	
	public ParticipationStatus Status { get; set; }

	public string? NonAttendanceReason  { get; set; }
	
	public bool IsGuest { get; set; }


	public static ActivityMember Create(Guid memberId,
		long activityId, bool isGuest, ParticipationStatus status)
	{
		return new ActivityMember
		{
			MemberId = memberId,
			ActivityId = activityId,
			CreatedDate = DateTime.UtcNow,
			IsGuest = isGuest,
			Status = status,
			NonAttendanceReason = null,
			IsActive = true,
		};
	}

	public void MakeParticipating()
	{
		Status = ParticipationStatus.Participating;
		NonAttendanceReason = null;
		UpdateDate = DateTime.UtcNow;

		CheckInvariant();
	}

	public void MakeNotParticipating(string reason)
	{
		Status = ParticipationStatus.NotParticipating;
		NonAttendanceReason = reason;
		UpdateDate = DateTime.UtcNow;

		CheckInvariant();
		
	}
	
	public void MakeGuest()
	{
		IsGuest = true;
		UpdateDate = DateTime.UtcNow;
	}

	public void MakeUnGuest()
	{
		IsGuest = false;
		UpdateDate = DateTime.UtcNow;
	}

	public void CheckInvariant()
	{
		switch (Status)
		{
			case ParticipationStatus.NotParticipating:
				if (NonAttendanceReason == null) 
					throw new ArgumentNullException($"NonAttendanceReason Can not be null");
				break;
			
			default:
				if (NonAttendanceReason != null) 
					throw new ArgumentException("NonAttendanceReason can not have value");
				break;
		}
	}

}
