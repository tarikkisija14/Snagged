using MediatR;

namespace Snagged.Application.Catalog.Auth.Commands.Register
{
    public class RegisterUserCommand : IRequest<int>
    {
       public RegisterDto User { get; set; }
    }
}
