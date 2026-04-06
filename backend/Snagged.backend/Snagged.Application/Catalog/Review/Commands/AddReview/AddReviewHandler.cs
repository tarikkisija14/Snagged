using MediatR;
using Microsoft.EntityFrameworkCore;
using Snagged.Application.Abstractions;
using Snagged.Application.Common.Exceptions;
using Snagged.Application.Common.Interfaces;

namespace Snagged.Application.Catalog.Review.Commands.AddReview
{
    public class AddReviewHandler(IAppDbContext ctx, ICurrentUserService currentUser)
        : IRequestHandler<AddReviewCommand, int>
    {
        public async Task<int> Handle(AddReviewCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = currentUser.UserId;

            if (currentUserId == request.ReviewedUserId)
                throw new InvalidOperationException("You cannot review yourself.");

            var reviewedUserExists = await ctx.Users
                .AnyAsync(u => u.Id == request.ReviewedUserId, cancellationToken);

            if (!reviewedUserExists)
                throw new SnaggedNotFoundException($"User with id {request.ReviewedUserId} not found.");

            var alreadyExists = await ctx.Reviews
                .AnyAsync(r => r.ReviewerId == currentUserId && r.ReviewedUserId == request.ReviewedUserId,
                    cancellationToken);

            if (alreadyExists)
                throw new InvalidOperationException("You have already reviewed this user. Please edit your existing review.");

            var review = new Snagged.Domain.Entities.Review
            {
                ReviewerId = currentUserId,
                ReviewedUserId = request.ReviewedUserId,
                Rating = request.Rating,
                Comment = request.Comment,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            ctx.Reviews.Add(review);

            ctx.Notifications.Add(new Snagged.Domain.Entities.Notification
            {
                UserId = request.ReviewedUserId,
                Message = $"You received a new {request.Rating}-star review!",
                NotificationType = "Review",
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            });

            await ctx.SaveChangesAsync(cancellationToken);

            await ReviewRatingService.RecalculateAsync(ctx, request.ReviewedUserId, cancellationToken);

            return review.Id;
        }
    }
}