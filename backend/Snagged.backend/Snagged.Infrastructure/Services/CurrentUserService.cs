using Microsoft.AspNetCore.Http;
using Snagged.Application.Common.Interfaces;
using System.Security.Claims;

namespace Snagged.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public int UserId { get; }

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            var userIdClaim = httpContextAccessor.HttpContext?
                .User?
                .FindFirst(ClaimTypes.NameIdentifier);

            if(userIdClaim != null && int.TryParse(userIdClaim.Value, out var userId))
            {
                UserId = userId;
            }
        }
    }
}
