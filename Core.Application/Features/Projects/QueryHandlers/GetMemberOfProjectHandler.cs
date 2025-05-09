using AutoMapper;
using Core.Application.Features.Projects.Query;
using Core.Domain;
using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.Projects.QueryHandlers
{
    public class GetMemberOfProjectHandler : ResponseHandler
        , IRequestHandler<GetMemberOfProjectQuery, Response<List<string>>>
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

        public async Task<Response<List<string>>> Handle(GetMemberOfProjectQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserServices.GetUserId();
            //check if user is the owner of project or not 
            var IsFor = await _unitOfWork.Projects.IsProjectForUser(request.ProjectId, userId, cancellationToken);
            if (!IsFor)
            {
                return BadRequest<List<string>>("your are not access to this part");
            }

            var membersName = await _unitOfWork.Requests
                .GetMemberOfProject(request.ProjectId, cancellationToken);

            return Success(membersName);
        }
    }
}
