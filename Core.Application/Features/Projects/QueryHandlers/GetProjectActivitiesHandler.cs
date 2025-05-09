using AutoMapper;
using Core.Application.DTOs.ActivityDTOs;
using Core.Application.Features.Projects.Query;
using Core.Domain;
using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.Projects.QueryHandlers
{
    public class GetProjectActivitiesHandler : ResponseHandler
        , IRequestHandler<GetProjectActivitiesQuery, Response<List<ActivityResponse>>>
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private ICurrentUserServices _currentUserServices;

        public GetProjectActivitiesHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserServices currentUserServices)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserServices = currentUserServices;
        }

        public async Task<Response<List<ActivityResponse>>> Handle(GetProjectActivitiesQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserServices.GetUserId();
            //check if user is the owner of project or not 
            var IsFor = await _unitOfWork.Projects.IsProjectForUser(request.ProjectId, userId, cancellationToken);
            if (!IsFor)
            {
                return BadRequest<List<ActivityResponse>>("your are not access to this part");
            }

            var activities = await _unitOfWork.Activities.GetProjectActivities(request.ProjectId, cancellationToken);
            var response = _mapper.Map<List<ActivityResponse>>(activities);

            return Success(response);
        }
    }
}
