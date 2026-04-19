using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snagged.Application.Catalog.Profiles;
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
           
            ProfileDto? result = null;

            try
            {
                result = await mediator.Send(new GetProfileQuery { UserId = currentUser.UserId });
            }
            catch (SnaggedNotFoundException)
            {
                // Profile missing — create it once. Any subsequent request will find it.
            }

            if (result is null)
            {
                try
                {
                    result = await mediator.Send(new CreateProfileCommand { UserId = currentUser.UserId });
                }
                catch (SnaggedNotFoundException ex)
                {
                    return NotFound(new { message = ex.Message });
                }
                catch (InvalidOperationException)
                {
                    
                    result = await mediator.Send(new GetProfileQuery { UserId = currentUser.UserId });
                }
            }

            return Ok(result);
        }
    }
}