using Core.Application.DTOs.UserRequestDTOs;
using FluentValidation;

namespace Core.Application.Validators
{
    public class SendRequestValidator : AbstractValidator<SendRequest>
    {
        public SendRequestValidator()
        {
            RuleFor(x => x.ActivityId)
                .NotEmpty()
                .Must(x => Guid.TryParse(x, out var result));

            RuleFor(x => x.Receiver)
                .NotNull()
                .NotEmpty();
                
        }
    }
}
