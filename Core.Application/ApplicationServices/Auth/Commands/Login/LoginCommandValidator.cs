using FluentValidation;

namespace Core.Application.ApplicationServices.Auth.Commands.Login
{
    public class LoginCommandValidator : AbstractValidator<LoginCommandRequest>
    {
        public LoginCommandValidator()
        {
            
        }
    }
}
