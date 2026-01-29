using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snagged.Application.Catalog.Items.Commands.AddItem;
using Snagged.Application.Catalog.Items.Commands.DeleteItem;
using Snagged.Application.Catalog.Items.Commands.UpdateItem;
using Snagged.Application.Catalog.Items.Dto;
using Snagged.Application.Catalog.Items.Queries.GetItems;
using Snagged.Application.Catalog.Items.Queries.GetItemsById;
using Snagged.Application.Catalog.Items.Queries.GetItemsFiltered;
using Snagged.Application.Catalog.Items.Queries.GetMyItems;
using Snagged.Application.Catalog.Items.Queries.GetPagedItems;
using Snagged.Application.Common.Paging;
using System.Runtime.CompilerServices;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


namespace Snagged.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ItemsController(IMediator mediator)
        {
            _mediator=mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetItems([FromQuery] GetItemsQuery query)
        {
            var items = await _mediator.Send(query);
            return Ok(items);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetItemById(int id)
        {
            var item =await _mediator.Send(new GetItemByIdQuery(id));

            if(item == null) 
                return NotFound();

            return Ok(item);

        }

        [HttpPost]
        public async Task<ActionResult<int>> Add([FromBody] AddItemCommand command)
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetItemById), new { id }, id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateItemCommand command)
        {
            command.Id = id; 
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteItemCommand { Id = id });
            return NoContent();
        }

        [HttpGet("paged")]
        public async Task<ActionResult<PageResult<ItemDto>>> GetPagedItems([FromQuery] GetPagedItemsQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("filtered")]
        public async Task<ActionResult<List<ItemDto>>> GetFilteredItems([FromQuery] GetItemsFilteredQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("filter-multiple")]
        public async Task<IActionResult> GetFilteredMultiple([FromBody] GetItemsFilteredQuery query)
        {
            try
            {
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("my")]
        [Authorize]
        public async Task<IActionResult> GetMyItems()
        {
            var result = await _mediator.Send(new GetMyItemsQuery());
            return Ok(result);
        }

    }
}
