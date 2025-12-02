using MediatR;
using Microsoft.AspNetCore.Mvc;
using Snagged.Application.Catalog.Countries.Commands.AddCountry;
using Snagged.Application.Catalog.Countries.Commands.DeleteCountry;
using Snagged.Application.Catalog.Countries.Commands.UpdateCountry;
using Snagged.Application.Catalog.Countries.Queries.GetCountries;
using Snagged.Application.Catalog.Countries.Queries.GetCountriesById;

namespace Snagged.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CountryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetCountriesQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetCountriesByIdQuery(id));

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddCountryCommand command)
        {
            int id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id }, null);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateCountryCommand command)
        {
            if (id != command.Id)
                return BadRequest("Id mismatch");

            var updated = await _mediator.Send(command);

            if (!updated)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _mediator.Send(new DeleteCountryCommand { Id = id });

                if (!deleted)
                    return NotFound("Country not found.");

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch
            {
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }
    }
}
