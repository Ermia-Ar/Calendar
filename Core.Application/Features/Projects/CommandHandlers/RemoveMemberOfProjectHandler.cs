using AutoMapper;
using Core.Application.Features.Exceptions;
using Core.Application.Features.Projects.Command;
using Core.Domain;
using Core.Domain.Entity;
using Core.Domain.Shared;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Threading;

namespace Core.Application.Features.Projects.CommandHandlers
{
    public class RemoveMemberOfProjectHandler : ResponseHandler
        , IRequestHandler<RemoveMemberOfProjectCommand, Response<string>>
    {

        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserServices _currentUserServices;

        public RemoveMemberOfProjectHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserServices currentUserServices)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserServices = currentUserServices;
        }

        public async Task<Response<string>> Handle(RemoveMemberOfProjectCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserServices.GetUserId();
            var project = await _unitOfWork.Projects.GetByIdAsync(request.ProjectId, cancellationToken);
            if (project.OwnerId != userId)
            {
                throw new BadRequestException("Only the owner of this project has access to this section.");

            }
            //
            var userRequests = await _unitOfWork.Requests.GetTableNoTracking()
                .Where(x => x.ProjectId == request.ProjectId 
                && x.Receiver == request.UserName)
                .ToListAsync(cancellationToken);

            if (!userRequests.Any())
            {
                throw new NotFoundException("No such member was found.");
            }

            _unitOfWork.Requests.DeleteRange(userRequests);
            await _unitOfWork.SaveChangeAsync(cancellationToken);
            return Deleted("");
        }
    }
}
