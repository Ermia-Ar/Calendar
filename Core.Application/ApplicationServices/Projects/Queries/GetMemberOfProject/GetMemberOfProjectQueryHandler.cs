using AutoMapper;
using Core.Application.ApplicationServices.Projects.Exceptions;
using Core.Domain;
using Mapster;
using MediatR;

namespace Core.Application.ApplicationServices.Projects.Queries.GetMemberOfProject
{
    public class GetMemberOfProjectQueryHandler
        : IRequestHandler<GetMemberOfProjectQueryRequest, List<GetMemberOfProjectQueryResponse>>
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private ICurrentUserServices _currentUserServices;

        public GetMemberOfProjectQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserServices currentUserServices)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserServices = currentUserServices;
        }

        public async Task<List<GetMemberOfProjectQueryResponse>> Handle(GetMemberOfProjectQueryRequest request, CancellationToken cancellationToken)
        {
            var userId = _currentUserServices.GetUserId();
            //check if user is the owner of project or not 
            var projectMembers = (await _unitOfWork.Requests
                .GetMemberOfProject(request.ProjectId, cancellationToken))
                .Adapt<List<GetMemberOfProjectQueryResponse>>();

            if (!projectMembers.Any(x => x.Id == userId))
            {
                throw new OnlyProjectMembersAllowedException();
            }
            return projectMembers;

        }
    }
}
