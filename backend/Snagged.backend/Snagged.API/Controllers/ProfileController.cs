using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Snagged.Application.Abstractions;
using Snagged.Application.Catalog.Profiles.Queries;

namespace Snagged.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/profile")]
    public class ProfileController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProfileController(IMediator mediator) {
            _mediator = mediator;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetProfile()
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                return Unauthorized("User ID not found in token.");
            }

            if (!int.TryParse(userIdClaim.Value, out int userId))
            {
                return BadRequest("Invalid User ID format.");
            }

            var result = await _mediator.Send(new GetProfileQuery { UserId = userId });

            if (result == null)
            {
                return NotFound(new { message = "Profile not found for this user." });
            }

            return Ok(result);
        }
    }
}
