using MediatR;
using Microsoft.AspNetCore.Mvc;
using Snagged.Application.Catalog.Auth.Commands.Register;
using Snagged.Application.Catalog.Auth.Commands.Login;

namespace Snagged.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command) //Deserialize the JSON request body into a LoginUserCommand object
        {
            var token = await _mediator.Send(command);
            return Ok(new { token });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
        {
            var token = await _mediator.Send(command);
            return Ok(new { token });
        }
    }
}
