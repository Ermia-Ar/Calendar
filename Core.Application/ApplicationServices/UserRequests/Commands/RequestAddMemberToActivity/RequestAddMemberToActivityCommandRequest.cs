using MediatR;

namespace Core.Application.ApplicationServices.UserRequests.Commands.RequestAddMemberToActivity;

public record class RequestAddMemberToActivityCommandRequest(
    string ActivityId ,
    string[] Receivers ,
    string? Message 

    ): IRequest;
