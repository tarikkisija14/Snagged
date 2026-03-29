using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snagged.Application.Catalog.Review.Commands.AddReview;
using Snagged.Application.Catalog.Review.Commands.DeleteReview;
using Snagged.Application.Catalog.Review.Commands.UpdateReview;
using Snagged.Application.Catalog.Review.Queries.GetReviewById;
using Snagged.Application.Catalog.Review.Queries.GetReviewsByReviewedUser;
using Snagged.Application.Catalog.Review.Queries.GetReviewsByReviewer;

namespace Snagged.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController(IMediator mediator) : ControllerBase
    {
        [HttpPost("add")]
        [Authorize]
        public async Task<IActionResult> Add([FromBody] AddReviewCommand cmd)
        {
            var id = await mediator.Send(cmd);
            return Ok(new { id });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var review = await mediator.Send(new GetReviewByIdQuery { Id = id });
            if (review == null) return NotFound(new { message = "Review not found" });
            return Ok(review);
        }

        [HttpGet("reviewer/{reviewerId}")]
        public async Task<IActionResult> GetByReviewer(int reviewerId)
        {
            var list = await mediator.Send(new GetReviewsByReviewerQuery { ReviewerId = reviewerId });
            return Ok(list);
        }

        [HttpGet("reviewed/{reviewedUserId}")]
        public async Task<IActionResult> GetByReviewedUser(int reviewedUserId)
        {
            var list = await mediator.Send(new GetReviewsByReviewedUserQuery { ReviewedUserId = reviewedUserId });
            return Ok(list);
        }

        [HttpPut("{id}/update")]
        [Authorize]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateReviewCommand cmd)
        {
            cmd.Id = id;
            var ok = await mediator.Send(cmd);
            if (!ok) return NotFound(new { message = "Review not found" });
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await mediator.Send(new DeleteReviewCommand { Id = id });
            if (!ok) return NotFound(new { message = "Review not found" });
            return NoContent();
        }
    }
}