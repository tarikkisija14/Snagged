using MediatR;
using Microsoft.AspNetCore.Mvc;
using Snagged.Application.Catalog.Subcategories.Commands.AddSubcategory;
using Snagged.Application.Catalog.Subcategories.Commands.DeleteSubCategory;
using Snagged.Application.Catalog.Subcategories.Commands.UpdateSubCategory;
using Snagged.Application.Catalog.Subcategories.Queries.GetSubcategories;
using Snagged.Application.Catalog.Subcategories.Queries.GetSubCategoriesById;

namespace Snagged.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubCategoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SubCategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll(int? categoryId)
        {
            var result = await _mediator.Send(new GetSubcategoriesQuery { CategoryId = categoryId });
            return Ok(result);
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetSubcategoryByIdQuery(id));

            if (result == null)
                return NotFound();

            return Ok(result);
        }

       
        [HttpPost]
        public async Task<IActionResult> Create(AddSubcategoryCommand command)
        {
            int id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id }, null);
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateSubcategoryCommand command)
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
                var result = await _mediator.Send(new DeleteSubcategoryCommand { Id = id });
                if (!result)
                    return NotFound("Subcategory not found.");

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
