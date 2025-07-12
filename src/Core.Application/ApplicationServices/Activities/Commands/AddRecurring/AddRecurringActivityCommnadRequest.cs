using Core.Domain.Enum;
using MediatR;

namespace Core.Application.ApplicationServices.Activities.Commands.AddRecurring;

public sealed record AddRecurringActivityCommnadRequest(
	 string Title,
	 string? Description,
	 DateTime StartDate,
	 TimeSpan? Duration,
	 TimeSpan? NotificationBefore,
	 ActivityCategory Category,
	 Guid[] MemberIds,
	 string? Message,
	 int Interval,
	 RecurrenceType Recurrence,
	 DateTime EndDate
	) : IRequest;
