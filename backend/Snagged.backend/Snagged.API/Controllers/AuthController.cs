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
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        {
            Console.WriteLine($"Command type: {command.GetType().FullName}");
            Console.WriteLine($"Implements IRequest<string>: {command is IRequest<string>}");
            Console.WriteLine($"Assembly: {command.GetType().Assembly.FullName}");

            var result = await _mediator.Send(command);
            return Ok(new { result });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(new { result });
        }
    }
}
