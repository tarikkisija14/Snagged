using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Snagged.Application.Abstractions;
using Snagged.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Snagged.Infrastructure.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(User user)
        {
            // Claims - pieces of info embedded inside the JWT
            var claims = new[]
             {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()), //user id
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),          // email
                new Claim(ClaimTypes.Role, user.Role?.RoleName ?? "User"),           // role name (default to "User")
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),  // unique token ID
            };

            //Create the signing key
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]) //  key from appsettings
            );

            //Create signing credentials
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Create the token
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],      // who issued the token
                audience: _configuration["Jwt:Audience"],  // who the token is for
                claims: claims,                             // claims defined above
                expires: DateTime.UtcNow.AddHours(2),       // token expiration
                signingCredentials: creds                   // signature
            );

            // 5️⃣ Return the token string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
