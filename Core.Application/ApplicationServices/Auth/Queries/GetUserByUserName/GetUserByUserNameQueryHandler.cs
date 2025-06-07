using AutoMapper;
using Core.Domain.Interfaces;
using Mapster;
using MediatR;

namespace Core.Application.ApplicationServices.Auth.Queries.GetUserByUserName;

public class GetUserByUserNameQueryHandler
    : IRequestHandler<GetUserByUserNameQueryRequest, GetUserByUserNameQueryResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetUserByUserNameQueryHandler(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<GetUserByUserNameQueryResponse> Handle(GetUserByUserNameQueryRequest request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.FindByUserName(request.UserName);
        var userResponse = user.Adapt<GetUserByUserNameQueryResponse>();
        return userResponse;
    }
}
