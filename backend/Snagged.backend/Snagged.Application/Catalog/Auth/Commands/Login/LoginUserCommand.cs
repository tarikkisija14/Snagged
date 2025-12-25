using MediatR;

namespace Snagged.Application.Catalog.Auth.Commands.Login
{
    public record LoginUserCommand(string email, string password) : IRequest<string>;
}