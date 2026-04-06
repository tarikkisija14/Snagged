using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using Snagged.Application.Common.Exceptions;

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

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.password, user.PasswordHash))
                throw new InvalidCredentialsException();

            var cartExists = await _context.Carts
                .AnyAsync(c => c.UserId == user.Id, ct);

            if (!cartExists)
            {
                _context.Carts.Add(new Snagged.Domain.Entities.Cart
                {
                    UserId = user.Id,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsSavedForLater = false
                });
                await _context.SaveChangesAsync(ct);
            }

            return _jwtService.GenerateToken(user);
        }
    }
}