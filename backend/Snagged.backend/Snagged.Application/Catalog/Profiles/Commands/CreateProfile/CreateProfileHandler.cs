using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using Snagged.Application.Common.Exceptions;
using Snagged.Domain.Entities;

namespace Snagged.Application.Catalog.Profiles.Commands.CreateProfile
{
    public class CreateProfileHandler(IAppDbContext ctx)
        : IRequestHandler<CreateProfileCommand, ProfileDto>
    {
        public async Task<ProfileDto> Handle(CreateProfileCommand request, CancellationToken ct)
        {
            
            var user = await ctx.Users
                .FirstOrDefaultAsync(u => u.Id == request.UserId, ct);

            if (user == null)
                throw new SnaggedNotFoundException($"User {request.UserId} not found.");

            
            var existing = await ctx.Profiles
                .FirstOrDefaultAsync(p => p.UserId == request.UserId, ct);

            if (existing != null)
                throw new InvalidOperationException($"Profile for user {request.UserId} already exists.");

            var profile = new Profile
            {
                UserId = user.Id,
                Username = $"{user.FirstName}{user.LastName}",
                PhoneNumber = "",
                Bio = "",
                AverageRating = 0,
                ReviewCount = 0
            };

            ctx.Profiles.Add(profile);
            await ctx.SaveChangesAsync(ct);

            return new ProfileDto
            {
                UserId = profile.UserId,
                Username = profile.Username,
                ProfileImageUrl = profile.ProfileImageUrl,
                Bio = profile.Bio,
                AverageRating = profile.AverageRating,
                ReviewCount = profile.ReviewCount
            };
        }
    }
}