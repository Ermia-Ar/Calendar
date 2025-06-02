using AutoMapper;
using Core.Application.DTOs.ActivityDTOs;
using Core.Application.Exceptions.Project;
using Core.Application.Features.Projects.Query;
using Core.Domain;
using MediatR;

namespace Core.Application.Features.Projects.QueryHandlers
{
    public class GetActivitiesOfProjectHandler 
        : IRequestHandler<GetActivitiesOfProjectQuery, List<ActivityResponse>>
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private ICurrentUserServices _currentUserServices;

        public GetActivitiesOfProjectHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserServices currentUserServices)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserServices = currentUserServices;
        }

        public async Task<List<ActivityResponse>> Handle(GetActivitiesOfProjectQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserServices.GetUserId();
            //check if user is the owner of project or not 
            var projectMembers = await _unitOfWork.Requests
                .GetMemberOfProject(request.ProjectId, cancellationToken);

            if (!projectMembers.Any(x => x.Id == userId))
            {
                throw new OnlyProjectMembersAllowedException();
            }
            var activities = await _unitOfWork.Activities.GetProjectActivities(request.ProjectId, cancellationToken);
            var response = _mapper.Map<List<ActivityResponse>>(activities);

            return response;
        }
    }
}
