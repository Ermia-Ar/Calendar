using AutoMapper;
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

        private IUnitOfWork _unitOfWork;
        private ICurrentUserServices _currentUserServices;
        private IMapper _mapper;

        public RemoveMemberOfProjectHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserServices currentUserServices)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserServices = currentUserServices;
        }

        public async Task<Response<string>> Handle(RemoveMemberOfProjectCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserServices.GetUserId();
            var isFor = await _unitOfWork.Projects.IsProjectForUser(request.ProjectId, userId, cancellationToken);
            if (!isFor)
            {
                return BadRequest<string>("you not access !!");
            }
            //
            var userRequest = await _unitOfWork.Requests.GetTableNoTracking(cancellationToken)
                .FirstOrDefaultAsync(x => x.ProjectId == request.ProjectId && x.Receiver == request.UserName);
            if (userRequest == null)
            {
                return BadRequest<string>("not exist");
            }

            _unitOfWork.Requests.Delete(userRequest);
            await _unitOfWork.SaveChangeAsync(cancellationToken);
            return NoContent<string>();
        }
    }
}
