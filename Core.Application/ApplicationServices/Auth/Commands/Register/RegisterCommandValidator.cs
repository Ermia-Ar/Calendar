using FluentValidation;

namespace Core.Application.ApplicationServices.Auth.Commands.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommandRequest>
{
    public RegisterCommandValidator()
    {
        
    }
}
