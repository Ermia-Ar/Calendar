using AutoMapper;
using Core.Application.Features.Projects.Command;
using Core.Domain;
using Core.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Application.Features.Projects.CommandHandlers
{
    public class DeleteProjectHandler : ResponseHandler
        , IRequestHandler<DeleteProjectCommand, Response<string>>
    {
        private IUnitOfWork _unitOfWork;
        private ICurrentUserServices _currentUserServices;
        private IMapper _mapper;

        public DeleteProjectHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserServices currentUserServices)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserServices = currentUserServices;
        }

        public async Task<Response<string>> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _unitOfWork.Projects.GetByIdAsync(request.ProjcetId, cancellationToken);
            if (project.OwnerId != _currentUserServices.GetUserId())
            {
                return BadRequest<string>("you can not delete this project !!");
            }

            await using var transaction = await _unitOfWork.Activities.BeginTransactionAsync();
            try
            {
                // delete from project table
                _unitOfWork.Projects.Delete(project);

                // delete all activity for this project 
                var activities = await _unitOfWork.Activities.GetTableNoTracking(cancellationToken)
                    .Where(x => x.ProjectId == request.ProjcetId)
                    .ToListAsync();
                _unitOfWork.Activities.DeleteRange(activities);

                // delete all request for this project 
                var requests = await _unitOfWork.Requests.GetTableNoTracking(cancellationToken)
                    .Where(x => x.ProjectId == request.ProjcetId)
                    .ToListAsync();
                _unitOfWork.Requests.DeleteRange(requests);

                await _unitOfWork.SaveChangeAsync(cancellationToken);
                await transaction.CommitAsync();
                return Deleted("");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest<string>(ex.Message);
            }
        }
    }
}