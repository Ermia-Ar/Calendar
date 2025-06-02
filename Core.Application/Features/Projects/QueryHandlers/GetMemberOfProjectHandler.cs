using AutoMapper;
using Core.Application.DTOs.UserDTOs;
using Core.Application.Exceptions.Project;
using Core.Application.Features.Projects.Query;
using Core.Domain;
using MediatR;

namespace Core.Application.Features.Projects.QueryHandlers
{
    public class GetMemberOfProjectHandler 
        : IRequestHandler<GetMemberOfProjectQuery, List<UserResponse>>
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private ICurrentUserServices _currentUserServices;

        public GetMemberOfProjectHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserServices currentUserServices)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserServices = currentUserServices;
        }

        public async Task<List<UserResponse>> Handle(GetMemberOfProjectQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserServices.GetUserId();
            //check if user is the owner of project or not 
            var projectMembers = await _unitOfWork.Requests
                .GetMemberOfProject(request.ProjectId, cancellationToken);

            if (!projectMembers.Any(x => x.Id == userId))
            {
                throw new OnlyProjectMembersAllowedException();
            }
            var response = _mapper.Map<List<UserResponse>>(projectMembers);
            return response;

        }
    }
}
