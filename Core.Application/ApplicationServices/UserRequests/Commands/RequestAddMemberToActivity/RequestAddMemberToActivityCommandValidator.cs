using FluentValidation;

namespace Core.Application.ApplicationServices.UserRequests.Commands.RequestAddMemberToActivity;

public class RequestAddMemberToActivityCommandValidator 
    : AbstractValidator<RequestAddMemberToActivityCommandRequest>
{
    public RequestAddMemberToActivityCommandValidator()
    {
        
    }
}
