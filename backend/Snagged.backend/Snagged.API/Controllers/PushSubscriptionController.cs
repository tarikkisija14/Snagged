using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Snagged.Application.Catalog.PushSubscriptions.Commands.Subscribe;
using Snagged.Application.Catalog.PushSubscriptions.Commands.Unsubscribe;
using Snagged.Application.Catalog.PushSubscriptions.Queries.GetPushSubscriptionStatus;
using Snagged.Application.Common.Helper;

namespace Snagged.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PushSubscriptionController(IMediator mediator, IOptions<VapidSettings> vapidOptions) : ControllerBase
    {
        [HttpGet("vapid-public-key")]
        [AllowAnonymous]
        public IActionResult GetVapidPublicKey()
        {
            return Ok(new { publicKey = vapidOptions.Value.PublicKey });
        }

        [HttpPost("subscribe")]
        public async Task<IActionResult> Subscribe([FromBody] SubscribePushCommand cmd)
        {
            await mediator.Send(cmd);
            return Ok();
        }

        [HttpPost("unsubscribe")]
        public async Task<IActionResult> Unsubscribe([FromBody] UnsubscribePushCommand cmd)
        {
            await mediator.Send(cmd);
            return Ok();
        }

        [HttpGet("status")]
        public async Task<IActionResult> GetStatus([FromQuery] string endpoint)
        {
            var isSubscribed = await mediator.Send(
                new GetPushSubscriptionStatusQuery { Endpoint = endpoint });
            return Ok(new { isSubscribed });
        }
    }
}