using AutoMapper;
using Core.Application.Features.Exceptions;
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
            var project = await _unitOfWork.Projects.GetByIdAsync(request.ProjectId, cancellationToken);
            if (project.OwnerId != userId)
            {
                throw new BadRequestException("Only the owner of this project has access to this section.");
            }

            var membersName = await _unitOfWork.Requests
                .GetMemberOfProject(request.ProjectId, cancellationToken);

            return Success(membersName);
        }
    }
}
