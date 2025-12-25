using MediatR;

namespace Snagged.Application.Catalog.Auth.Commands.Register
{
    public record RegisterUserCommand(
          string Email,
          string Password,
          string FirstName,
          string LastName
      ) : IRequest<string>;
}
