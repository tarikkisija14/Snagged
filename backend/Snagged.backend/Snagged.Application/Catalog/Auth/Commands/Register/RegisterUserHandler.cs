using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using Snagged.Application.Common.Exceptions;
using Snagged.Domain.Entities;

namespace Snagged.Application.Catalog.Auth.Commands.Register
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, string>
    {
        private readonly IAppDbContext _context;
        private readonly IJwtService _jwtService;

        public RegisterUserHandler(IAppDbContext context, IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        public async Task<string> Handle(RegisterUserCommand request, CancellationToken ct)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email, ct);

            if (existingUser != null)
                throw new SnaggedConflictException("Email already registered.");

            var defaultRole = await _context.Roles
                 .FirstOrDefaultAsync(r => r.RoleName == "User", ct);

            if (defaultRole == null)
                throw new Exception("Default role not found.");

            var user = new User
            {
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                FirstName = request.FirstName,
                LastName = request.LastName,
                CreatedAt = DateTime.UtcNow,
                RoleId = defaultRole.Id
            };

            var cart = new Snagged.Domain.Entities.Cart
            {
                User = user, 
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsSavedForLater = false
            };

            var profile = new Profile
            {
                User = user, 
                Username = $"{user.FirstName}{user.LastName}",
                PhoneNumber = "",
                Bio = "",
                AverageRating = 0,
                ReviewCount = 0
            };

            _context.Users.Add(user);
            _context.Carts.Add(cart);
            _context.Profiles.Add(profile);

            await _context.SaveChangesAsync(ct);

            var token = _jwtService.GenerateToken(user);
            return token;
        }
    }
}