using FluentValidation;

namespace Snagged.Application.Catalog.Auth.Commands.Login
{
    public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
    {
        public LoginUserCommandValidator()
        {
            RuleFor(x => x.email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("Valid email is required.");

            RuleFor(x => x.password)
                .NotEmpty()
                .MinimumLength(6)
                .WithMessage("Password must be at least 6 characters.");
        }
    }
}