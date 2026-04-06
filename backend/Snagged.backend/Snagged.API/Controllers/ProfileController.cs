using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snagged.Application.Catalog.Profiles.Commands.CreateProfile;
using Snagged.Application.Catalog.Profiles.Queries;
using Snagged.Application.Common.Exceptions;
using Snagged.Application.Common.Interfaces;

namespace Snagged.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/profile")]
    public class ProfileController(IMediator mediator, ICurrentUserService currentUser) : ControllerBase
    {
        [HttpGet("me")]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                var result = await mediator.Send(new GetProfileQuery { UserId = currentUser.UserId });
                return Ok(result);
            }
            catch (SnaggedNotFoundException)
            {
                
                try
                {
                    var created = await mediator.Send(new CreateProfileCommand { UserId = currentUser.UserId });
                    return Ok(created);
                }
                catch (SnaggedNotFoundException ex)
                {
                    return NotFound(new { message = ex.Message });
                }
            }
        }
    }
}