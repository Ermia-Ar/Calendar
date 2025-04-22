using Core.Application.DTOs.AuthDTOs;
using FluentValidation;

namespace Core.Application.Validators
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.UserNameOrEmail)
                .NotEmpty();

            RuleFor(x => x.Password)
                .MinimumLength(8);
        }
    }
}
