using MediatR;
using Microsoft.AspNetCore.Mvc;
using Snagged.Application.Catalog.Categories.Commands.AddCategory;
using Snagged.Application.Catalog.Categories.Commands.DeleteCategory;
using Snagged.Application.Catalog.Categories.Commands.UpdateCategory;
using Snagged.Application.Catalog.Categories.Queries.GetCategories;
using Snagged.Application.Catalog.Categories.Queries.GetCategoryById;

namespace Snagged.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetCategoriesQuery());
            return Ok(result);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetCategoryByIdQuery(id));

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddCategoryCommand command)
        {
            int id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id }, null);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateCategoryCommand command)
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
            var deleted = await _mediator.Send(new DeleteCategoryCommand { Id = id });

            if (!deleted)
                return NotFound();

            return NoContent();
        }

    }
}
