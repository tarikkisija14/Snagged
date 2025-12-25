using Snagged.Domain.Entities;

namespace Snagged.Application.Abstractions
{
    public interface IJwtService 
    {
        string GenerateToken(User user);
    }
}
