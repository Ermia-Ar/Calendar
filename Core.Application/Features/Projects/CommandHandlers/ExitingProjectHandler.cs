using AutoMapper;
using Core.Application.Features.Projects.Command;
using Core.Domain;
using Core.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Application.Features.Projects.CommandHandlers
{
    public class ExitingProjectHandler : ResponseHandler
        , IRequestHandler<ExitingProjectCommand, Response<string>>
    {

        private IUnitOfWork _unitOfWork;
        private ICurrentUserServices _currentUserServices;
        private IMapper _mapper;

        public ExitingProjectHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserServices currentUserServices)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserServices = currentUserServices;
        }

        public async Task<Response<string>> Handle(ExitingProjectCommand request, CancellationToken cancellationToken)
        {
            var requests = await _unitOfWork.Requests.GetTableNoTracking(cancellationToken)
               .Where(x => x.ProjectId == request.ProjectId
               && x.Receiver == _currentUserServices.GetUserName())
               .ToListAsync();

            _unitOfWork.Requests.DeleteRange(requests);
            await _unitOfWork.SaveChangeAsync(cancellationToken);
            return Deleted("");

        }
    }
}
