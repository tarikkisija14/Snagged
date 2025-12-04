using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Snagged.Application.Catalog.Review.Commands.AddReview;
using Snagged.Application.Catalog.Review.Commands.DeleteReview;
using Snagged.Application.Catalog.Review.Commands.UpdateReview;
using Snagged.Application.Catalog.Review.Queries.GetReviewById;
using Snagged.Application.Catalog.Review.Queries.GetReviewsByReviewedUser;
using Snagged.Application.Catalog.Review.Queries.GetReviewsByReviewerQuery;

namespace Snagged.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReviewController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddReviewCommand cmd)
        {
            try
            {
                var id = await _mediator.Send(cmd);
                return Ok(new { id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var review = await _mediator.Send(new GetReviewByIdQuery { Id = id });
                if (review == null) return NotFound(new { message = "Review not found" });
                return Ok(review);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        [HttpGet("reviewer/{reviewerId}")]
        public async Task<IActionResult> GetByReviewer(int reviewerId)
        {
            try
            {
                var list = await _mediator.Send(new GetReviewsByReviewerQuery { ReviewerId = reviewerId });
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        [HttpGet("reviewed/{reviewedUserId}")]
        public async Task<IActionResult> GetByReviewedUser(int reviewedUserId)
        {
            try
            {
                var list = await _mediator.Send(new GetReviewsByReviewedUserQuery { ReviewedUserId = reviewedUserId });
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        [HttpPut("{id}/update")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateReviewCommand cmd)
        {
            try
            {
                cmd.Id = id;
                var ok = await _mediator.Send(cmd);
                if (!ok) return NotFound(new { message = "Review not found" });
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var ok = await _mediator.Send(new DeleteReviewCommand { Id = id });
                if (!ok) return NotFound(new { message = "Review not found" });
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message, stackTrace = ex.StackTrace });
            }
        }
    }
}
