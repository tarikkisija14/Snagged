using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snagged.Application.Catalog.Review;
using Snagged.Application.Catalog.Review.Commands.AddReview;
using Snagged.Application.Catalog.Review.Commands.DeleteReview;
using Snagged.Application.Catalog.Review.Commands.UpdateReview;
using Snagged.Application.Catalog.Review.Queries.GetMyReview;
using Snagged.Application.Catalog.Review.Queries.GetReviewById;
using Snagged.Application.Catalog.Review.Queries.GetReviewsByReviewedUser;
using Snagged.Application.Catalog.Review.Queries.GetReviewsByReviewer;
using Snagged.Application.Catalog.Review.Queries.GetReviewsByItem;
using Snagged.Application.Common.Exceptions;
using Snagged.Application.Common.Paging;

namespace Snagged.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController(IMediator mediator) : ControllerBase
    {
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add([FromBody] AddReviewCommand cmd)
        {
            try
            {
                var id = await mediator.Send(cmd);
                return CreatedAtAction(nameof(GetById), new { id }, new { id });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (SnaggedNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPut("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateReviewCommand cmd)
        {
            cmd.Id = id;
            try
            {
                var ok = await mediator.Send(cmd);
                if (!ok) return NotFound(new { message = "Review not found." });
                return NoContent();
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var ok = await mediator.Send(new DeleteReviewCommand { Id = id });
                if (!ok) return NotFound(new { message = "Review not found." });
                return NoContent();
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var review = await mediator.Send(new GetReviewByIdQuery { Id = id });
            if (review == null) return NotFound(new { message = "Review not found." });
            return Ok(review);
        }

        [HttpGet("user/{reviewedUserId:int}/paged")]
        public async Task<IActionResult> GetPaged(
            int reviewedUserId,
            [FromQuery] PageRequest paging,
            [FromQuery] ReviewSortOrder sortOrder = ReviewSortOrder.Newest)
        {
            var result = await mediator.Send(new GetReviewsPagedQuery
            {
                ReviewedUserId = reviewedUserId,
                Paging = paging,
                SortOrder = sortOrder
            });
            return Ok(result);
        }

        [HttpGet("reviewed/{reviewedUserId:int}")]
        public async Task<IActionResult> GetByReviewedUser(int reviewedUserId)
        {
            var list = await mediator.Send(new GetReviewsByReviewedUserQuery { ReviewedUserId = reviewedUserId });
            return Ok(list);
        }

        [HttpGet("reviewer/{reviewerId:int}")]
        public async Task<IActionResult> GetByReviewer(int reviewerId)
        {
            var list = await mediator.Send(new GetReviewsByReviewerQuery { ReviewerId = reviewerId });
            return Ok(list);
        }

        [HttpGet("my/{reviewedUserId:int}")]
        [Authorize]
        public async Task<IActionResult> GetMyReview(int reviewedUserId)
        {
            var review = await mediator.Send(new GetMyReviewForUserQuery { ReviewedUserId = reviewedUserId });
            if (review == null) return NoContent();
            return Ok(review);
        }
    }
}