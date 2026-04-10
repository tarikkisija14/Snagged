using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snagged.Application.Catalog.Orders.Commands;
using Snagged.Application.Catalog.Orders.Commands.CreateOrder;
using Snagged.Application.Catalog.Orders.Commands.DeleteOrder;
using Snagged.Application.Catalog.Orders.Commands.UpdateOrder;
using Snagged.Application.Catalog.Orders.Queries.GetOrders;
using Snagged.Application.Catalog.Orders.Queries.GetOrdersById;
using Snagged.Application.Catalog.Orders.Queries.GetOrdersPaged;
using Snagged.Application.Common.Exceptions;
using Snagged.Application.Common.Paging;

namespace Snagged.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    
    public class OrdersController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<OrderDto>>> GetOrders([FromQuery] GetOrdersQuery query)
        {
            var result = await mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderDto>> GetOrderById(int id)
        {
            try
            {
                var result = await mediator.Send(new GetOrdersByIdQuery { Id = id });
                return Ok(result);
            }
            
            catch (SnaggedNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateOrder([FromBody] CreateOrderCommand command)
        {
            var orderId = await mediator.Send(command);
            return CreatedAtAction(nameof(GetOrderById), new { id = orderId }, orderId);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] UpdateOrderCommand command)
        {
            if (id != command.Id)
                return BadRequest(new { error = "ID mismatch." });

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
        public async Task<IActionResult> DeleteOrder(int id)
        {
            try
            {
                await mediator.Send(new DeleteOrderCommand { Id = id });
                return NoContent();
            }
            catch (SnaggedNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpGet("paged")]
        public async Task<ActionResult<PageResult<OrderDto>>> GetPagedOrders([FromQuery] GetOrdersPagedQuery query)
        {
            var result = await mediator.Send(query);
            return Ok(result);
        }
    }
}