using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;

namespace Snagged.Application.Catalog.Auth.Commands.Login
{
    public class LoginUserHandler : IRequestHandler<LoginUserCommand, string>
    {
        private readonly IAppDbContext _context;
        private readonly IJwtService _jwtService;

        public LoginUserHandler(IAppDbContext context, IJwtService jwtService )
        {
            _context = context;
            _jwtService = jwtService;
        }

        public async Task<string> Handle(LoginUserCommand request, CancellationToken ctk)
        {
            //Find user by email
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.User.Email, ctk);

            if (user == null)
                throw new Exception("Invalid email or password.");

            //Password verification
            bool valid = BCrypt.Net.BCrypt.Verify(request.User.Password, user.Password);
            if (!valid)
                throw new Exception("Invalid email or password.");

            //Generating a token
            var token = _jwtService.GenerateToken(user);
            return token;

        }
    }
}
