using MediatR;
using Microsoft.AspNetCore.Mvc;
using Snagged.Application.Commom.Paging;
using Snagged.Application.Items.Commands.AddItem;
using Snagged.Application.Items.Commands.DeleteItem;
using Snagged.Application.Items.Commands.UpdateItem;
using Snagged.Application.Items.Queries.GetItems;
using Snagged.Application.Items.Queries.GetItemsById;
using Snagged.Application.Items.Queries.GetItemsFiltered;
using Snagged.Application.Items.Queries.GetPagedItems;
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

    }
}
