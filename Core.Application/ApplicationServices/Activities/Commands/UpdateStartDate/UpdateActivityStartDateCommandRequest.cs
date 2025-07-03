using MediatR;

namespace Core.Application.ApplicationServices.Activities.Commands.UpdateStartDate;



public sealed record UpdateActivityStartDateCommandRequest(
	string activityId,
	DateTime NewStartDate

) : IRequest;
