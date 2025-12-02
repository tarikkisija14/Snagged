using MediatR;
using Microsoft.AspNetCore.Mvc;
using Snagged.Application.Catalog.Adresses.Commands.AddAddress;
using Snagged.Application.Catalog.Adresses.Commands.DeleteAddress;
using Snagged.Application.Catalog.Adresses.Commands.UpdateAddress;
using Snagged.Application.Catalog.Adresses.Queries.GetAdresses;
using Snagged.Application.Catalog.Adresses.Queries.GetAdressesById;

namespace Snagged.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AddressController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AddressController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetAll(int? userId)
        {
            var result = await _mediator.Send(new GetAddressesQuery { UserId = userId });
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetAddressByIdQuery(id));
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddAddressCommand cmd)
        {
            var id = await _mediator.Send(cmd);
            return CreatedAtAction(nameof(GetById), new { id }, null);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateAddressCommand cmd)
        {
            if (id != cmd.Id)
                return BadRequest("Id mismatch");

            var ok = await _mediator.Send(cmd);
            if (!ok) return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _mediator.Send(new DeleteAddressCommand { Id = id });
                if (!result) return NotFound();
                return NoContent();
            }
            catch
            {
                return BadRequest("Cannot delete address.");
            }
        }
    }
}
