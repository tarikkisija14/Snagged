using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            try
            {
                var result = await _mediator.Send(new GetCategoriesQuery());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _mediator.Send(new GetCategoryByIdQuery(id));
                if (result == null)
                    return NotFound();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddCategoryCommand command)
        {
            try
            {
                int id = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetById), new { id }, null);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateCategoryCommand command)
        {
            if (id != command.Id)
                return BadRequest("Id mismatch");

            try
            {
                var updated = await _mediator.Send(command);
                if (!updated)
                    return NotFound();
                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                
                return Conflict("Unable to update category: " + ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _mediator.Send(new DeleteCategoryCommand { Id = id });
                if (!deleted)
                    return NotFound();
                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                
                return Conflict("Unable to delete category: " + ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
