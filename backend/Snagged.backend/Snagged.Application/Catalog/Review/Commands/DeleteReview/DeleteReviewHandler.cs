using MediatR;
using Snagged.Application.Abstractions;
using Stripe.Forwarding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Snagged.Application.Catalog.Review.Commands.DeleteReview
{
    public class DeleteReviewHandler(IAppDbContext ctx) : IRequestHandler<DeleteReviewCommand, bool>
    {
        public async Task<bool> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
        {
            var review = await ctx.Reviews.FindAsync(request.Id);
            if (review == null) return false;

            ctx.Reviews.Remove(review);
            await ctx.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
