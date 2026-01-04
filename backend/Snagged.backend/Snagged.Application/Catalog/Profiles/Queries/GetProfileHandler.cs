using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Profiles.Queries
{
    public class GetProfileHandler : IRequestHandler<GetProfileQuery, ProfileDto>
    {
        private readonly IAppDbContext _context;

        public GetProfileHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<ProfileDto?> Handle(GetProfileQuery request, CancellationToken ct)
        {
            var profile = await _context.Profiles
                .Where(p => p.UserId == request.UserId)
                .Select(p => new ProfileDto
                {
                    Username = p.Username,
                    ProfileImageUrl = p.ProfileImageUrl,
                    Bio = p.Bio,
                    AverageRating = p.AverageRating,
                    ReviewCount = p.ReviewCount
                }).FirstOrDefaultAsync(ct);

            return profile;
        }
    }
    }
