using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;

namespace Snagged.Application.Catalog.Review
{
    
    public static class ReviewRatingService
    {
        public static async Task RecalculateAsync(IAppDbContext ctx, int reviewedUserId, CancellationToken ct)
        {
            var profile = await ctx.Profiles.FirstOrDefaultAsync(p => p.UserId == reviewedUserId, ct);
            if (profile == null) return;

            var stats = await ctx.Reviews
                .Where(r => r.ReviewedUserId == reviewedUserId)
                .GroupBy(_ => 1)
                .Select(g => new { Avg = g.Average(r => (double)r.Rating), Count = g.Count() })
                .FirstOrDefaultAsync(ct);

            profile.AverageRating = (decimal)(stats?.Avg ?? 0);
            profile.ReviewCount = stats?.Count ?? 0;

            await ctx.SaveChangesAsync(ct);
        }
    }
}