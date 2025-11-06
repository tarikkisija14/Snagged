using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using Snagged.Domain.Entities;

namespace Snagged.Application.Catalog.Auth.Commands.Register
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, int>
    {
        private readonly IAppDbContext _context;

        public RegisterUserHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(RegisterUserCommand request, CancellationToken ctk)
        {
            //check if user with the same email already exists
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.User.Email, ctk);

            if (existingUser != null)
                throw new InvalidOperationException("Email already registered.");

            //hash the password
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.User.Password);

            var user = new User
            {
                Email = request.User.Email,
                Password = hashedPassword
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync(ctk);

            return user.Id;
        }
    }
}
