using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using Snagged.Application.Common.Exceptions;

namespace Snagged.Application.Catalog.Profiles.Queries
{
    public class GetProfileHandler : IRequestHandler<GetProfileQuery, ProfileDto>
    {
        private readonly IAppDbContext _context;

        public GetProfileHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<ProfileDto> Handle(GetProfileQuery request, CancellationToken ct)
        {
            var profile = await _context.Profiles
                .Where(p => p.UserId == request.UserId)
                .Select(p => new ProfileDto
                {
                    UserId = p.UserId,
                    Username = p.Username,
                    ProfileImageUrl = p.ProfileImageUrl,
                    Bio = p.Bio,
                    AverageRating = p.AverageRating,
                    ReviewCount = p.ReviewCount
                })
                .FirstOrDefaultAsync(ct);

            if (profile == null)
                throw new SnaggedNotFoundException($"Profile for user {request.UserId} not found.");

            return profile;
        }
    }
}