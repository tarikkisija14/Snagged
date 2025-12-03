using MediatR;
using Microsoft.AspNetCore.Mvc;
using Snagged.Application.Catalog.Cities.Commands.AddCity;
using Snagged.Application.Catalog.Cities.Commands.DeleteCity;
using Snagged.Application.Catalog.Cities.Commands.UpdateCity;
using Snagged.Application.Catalog.Cities.Queries.GetAllCities;
using Snagged.Application.Catalog.Cities.Queries.GetCityById;

namespace Snagged.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CityController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CityController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int? countryId)
        {
            var result = await _mediator.Send(new GetCitiesQuery { CountryId = countryId });
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetCityByIdQuery(id));
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddCityCommand command)
        {
            int id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id }, null);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateCityCommand command)
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
                var result = await _mediator.Send(new DeleteCityCommand { Id = id });
                if (!result)
                    return NotFound("City not found.");

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }
    }
}
