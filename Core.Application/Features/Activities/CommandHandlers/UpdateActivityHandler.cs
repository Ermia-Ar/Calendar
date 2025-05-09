using AutoMapper;
using Core.Application.DTOs.ActivityDTOs;
using Core.Application.Features.Activities.Commands;
using Core.Domain.Entity;
using Core.Domain;
using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.Activities.CommandHandlers
{
    public class UpdateActivityHandler : ResponseHandler
        , IRequestHandler<UpdateActivityCommand, Response<ActivityResponse>>
    {
        public IUnitOfWork _unitOfWork;
        public ICurrentUserServices _currentUser;
        public IMapper _mapper;

        public UpdateActivityHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUser, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task<Response<ActivityResponse>> Handle(UpdateActivityCommand request, CancellationToken cancellationToken)
        {
            var isFor = await _unitOfWork.Activities.IsActivityForUser(request.UpdateActivityRequest.Id, _currentUser.GetUserId(), cancellationToken);
            if (!isFor)
            {
                return NotFound<ActivityResponse>("Activity not found !!");
            }
            // update activity
            try
            {
                var updateActivity = _mapper.Map<Activity>(request.UpdateActivityRequest);
                var activity = await _unitOfWork.Activities.UpdateActivity(updateActivity, cancellationToken);
                //map to activityResponse
                var response = _mapper.Map<ActivityResponse>(activity);
                await _unitOfWork.SaveChangeAsync(cancellationToken);
                return Success(response);
            }
            catch (Exception ex)
            {
                return NotFound<ActivityResponse>(ex.Message);
            }
        }
    }
}