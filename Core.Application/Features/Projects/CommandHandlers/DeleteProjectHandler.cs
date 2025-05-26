using AutoMapper;
using Core.Application.Features.Exceptions;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserServices _currentUserServices;
        private readonly IMapper _mapper;

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
                throw new BadRequestException("you can not delete this project !!");
            }

            await using var transaction = await _unitOfWork.Activities.BeginTransactionAsync();
            try
            {

                // delete from comments table
                var comments = await _unitOfWork.Comments.GetTableAsTracking()
                    .Where(x => x.ProjectId == request.ProjcetId)
                    .ToListAsync(cancellationToken);
                _unitOfWork.Comments.DeleteRange(comments);

                // delete all activity for this project 
                var activities = await _unitOfWork.Activities.GetTableNoTracking()
                    .Where(x => x.ProjectId == request.ProjcetId)
                    .ToListAsync(cancellationToken);
                _unitOfWork.Activities.DeleteRange(activities);

                // delete all request for this project 
                var requests = await _unitOfWork.Requests.GetTableNoTracking()
                    .Where(x => x.ProjectId == request.ProjcetId)
                    .ToListAsync(cancellationToken);
                _unitOfWork.Requests.DeleteRange(requests);

                // delete from projects table
                _unitOfWork.Projects.Delete(project);

                await _unitOfWork.SaveChangeAsync(cancellationToken);
                await transaction.CommitAsync();
                return Deleted("");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new BadRequestException("something wrong!");
            }
        }
    }
}