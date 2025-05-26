using AutoMapper;
using Core.Application.Features.Exceptions;
using Core.Application.Features.Projects.Command;
using Core.Domain;
using Core.Domain.Entity;
using Core.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Application.Features.Projects.CommandHandlers
{
    public class ExitingProjectHandler : ResponseHandler
        , IRequestHandler<ExitingProjectCommand, Response<string>>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserServices _currentUserServices;
        private readonly IMapper _mapper;

        public ExitingProjectHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserServices currentUserServices)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserServices = currentUserServices;
        }

        public async Task<Response<string>> Handle(ExitingProjectCommand request, CancellationToken cancellationToken)
        {
            var requests = await _unitOfWork.Requests.GetTableNoTracking()
               .Where(x => x.ProjectId == request.ProjectId
                && x.Receiver.Id == _currentUserServices.GetUserId()
                && x.Status == Domain.Enum.RequestStatus.Accepted)
               .ToListAsync();

            if (!requests.Any())
            {
                throw new NotFoundException("You are not a member of this project.");
            }

            _unitOfWork.Requests.DeleteRange(requests);
            await _unitOfWork.SaveChangeAsync(cancellationToken);
            return Deleted("");

        }
    }
}
