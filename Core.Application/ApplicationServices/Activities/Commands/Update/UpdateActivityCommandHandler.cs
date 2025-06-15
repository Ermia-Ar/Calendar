using AutoMapper;
using Core.Application.ApplicationServices.Activities.Exceptions;
using Core.Domain.Entity.Activities;
using Core.Domain.Interfaces;
using Mapster;
using MediatR;

namespace Core.Application.ApplicationServices.Activities.Commands.UpdateActivity;

public sealed class UpdateActivityCommandHandler(IUnitOfWork unitOfWork
    ,ICurrentUserServices currentUser
    ,IMapper mapper)
    : IRequestHandler<UpdateActivityCommandRequest>
{
    public readonly IUnitOfWork _unitOfWork = unitOfWork;
    public readonly IMapper _mapper = mapper;
    public readonly ICurrentUserServices _currentUser = currentUser;

    public async Task Handle(UpdateActivityCommandRequest request, CancellationToken cancellationToken)
    {
        var activity = await _unitOfWork.Activities
            .FindById(request.Id, cancellationToken);

        if (activity.UserId != _currentUser.GetUserId())
        {
            throw new OnlyActivityCreatorAllowedException();
        }

        // update activity
        var updateActivity = request.Adapt<Activity>();

        _unitOfWork.Activities.Update(updateActivity);

        //map to activityResponse
        await _unitOfWork.SaveChangeAsync(cancellationToken);
    }
}