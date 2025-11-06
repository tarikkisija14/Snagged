using MediatR;

namespace Snagged.Application.Catalog.Auth.Commands.Login
{
    public class LoginUserCommand : IRequest<string>
    {
       public LoginDto User {  get; set; }
    }
}
