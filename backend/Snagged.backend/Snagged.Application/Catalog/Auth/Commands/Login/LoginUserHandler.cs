using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using Snagged.Application.Common.Exceptions;
using Snagged.Application.Catalog.Auth.Commands.Login;

namespace Snagged.Application.Catalog.Auth.Commands.Login
{
    public class LoginUserHandler : IRequestHandler<LoginUserCommand, string>
    {
        private readonly IAppDbContext _context;
        private readonly IJwtService _jwtService;

        public LoginUserHandler(IAppDbContext context, IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        public async Task<string> Handle(LoginUserCommand request, CancellationToken ct)
        {
           var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == request.email, ct);

            if (user == null ||
            !BCrypt.Net.BCrypt.Verify(request.password, user.PasswordHash))
            {
                throw new InvalidCredentialsException();
            }

            var existingCart = await _context.Carts
            .FirstOrDefaultAsync(c => c.UserId == user.Id && !c.IsSavedForLater, ct);

            if (existingCart == null)
            {
                var cart = new Snagged.Domain.Entities.Cart
                {
                    UserId = user.Id,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsSavedForLater = false
                };

                _context.Carts.Add(cart);
                await _context.SaveChangesAsync(ct);
            }

            return _jwtService.GenerateToken(user);
        }
    }
}