using FluentValidation;

namespace Core.Application.ApplicationServices.Auth.Commands.Login
{
    public class LoginCommandValidator : AbstractValidator<LoginCommandRequest>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.UserNameOrEmail)
                .MinimumLength(3);

            RuleFor(x => x.Password)
                .MinimumLength(8);
        }
    }
}
