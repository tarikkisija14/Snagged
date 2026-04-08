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
using Snagged.Application.Catalog.Items.Queries.GetItemSuggestions;
using Snagged.Application.Catalog.Items.Queries.GetMyItems;
using Snagged.Application.Catalog.Items.Queries.GetPagedItems;
using Snagged.Application.Common.Exceptions;
using Snagged.Application.Common.Paging;

namespace Snagged.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetItems([FromQuery] GetItemsQuery query)
        {
            var items = await mediator.Send(query);
            return Ok(items);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ItemDto>> GetItemById(int id)
        {
            try
            {
                var item = await mediator.Send(new GetItemByIdQuery(id));
                return Ok(item);
            }
            catch (SnaggedNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<int>> AddItem([FromBody] AddItemCommand command)
        {
            var id = await mediator.Send(command);
            return CreatedAtAction(nameof(GetItemById), new { id }, new { id });
        }

        [HttpPut("{id:int}")]
        [Authorize]
        public async Task<IActionResult> UpdateItem(int id, [FromBody] UpdateItemCommand command)
        {
            command.Id = id;
            try
            {
                await mediator.Send(command);
                return NoContent();
            }
            catch (SnaggedNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize]
        public async Task<IActionResult> DeleteItem(int id)
        {
            try
            {
                await mediator.Send(new DeleteItemCommand { Id = id });
                return NoContent();
            }
            catch (SnaggedNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpGet("paged")]
        public async Task<ActionResult<PageResult<ItemDto>>> GetPagedItems([FromQuery] GetPagedItemsQuery query)
        {
            var result = await mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("filtered")]
        public async Task<ActionResult<PageResult<ItemDto>>> GetFilteredItems([FromQuery] GetItemsFilteredQuery query)
        {
            var result = await mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("my")]
        [Authorize]
        public async Task<IActionResult> GetMyItems()
        {
            var result = await mediator.Send(new GetMyItemsQuery());
            return Ok(result);
        }

        [HttpGet("suggestions")]
        public async Task<IActionResult> GetSuggestions([FromQuery] GetItemSuggestionsQuery request)
        {
            var result = await mediator.Send(request);
            return Ok(result);
        }
    }
}