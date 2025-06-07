using AutoMapper;
using Core.Application.ApplicationServices.Activities.Queries.GetById;
using Core.Application.ApplicationServices.Projects.Exceptions;
using Core.Domain.Entity;
using Core.Domain.Interfaces;
using Mapster;
using MediatR;

namespace Core.Application.ApplicationServices.Projects.Queries.GetActivitiesOfProject
{
    public class GetActivitiesOfProjectQueryHandler
        : IRequestHandler<GetActivitiesOfProjectQueryRequest, List<GetActivityByIdQueryResponse>>
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private ICurrentUserServices _currentUserServices;

        public GetActivitiesOfProjectQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserServices currentUserServices)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserServices = currentUserServices;
        }

        public async Task<List<GetActivityByIdQueryResponse>> Handle(GetActivitiesOfProjectQueryRequest request, CancellationToken cancellationToken)
        {
            var userId = _currentUserServices.GetUserId();
            //check if user is the owner of project or not 
            var projectMembers = (await _unitOfWork.Requests
                .GetMemberOfProject(request.ProjectId, cancellationToken))
                .Adapt<List<User>>();

            if (!projectMembers.Any(x => x.Id == userId))
            {
                throw new OnlyProjectMembersAllowedException();
            }
            var activities = await _unitOfWork.Activities
                .GetProjectActivities(request.ProjectId, cancellationToken);

            var response = activities.Adapt<List<GetActivityByIdQueryResponse>>();

            return response;
        }
    }
}
