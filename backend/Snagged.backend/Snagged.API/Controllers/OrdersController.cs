using MediatR;
using Microsoft.AspNetCore.Mvc;
using Snagged.Application.Catalog.Orders.Commands;
using Snagged.Application.Catalog.Orders.Commands.CreateOrder;
using Snagged.Application.Catalog.Orders.Commands.DeleteOrder;
using Snagged.Application.Catalog.Orders.Commands.UpdateOrder;
using Snagged.Application.Catalog.Orders.Queries.GetOrders;
using Snagged.Application.Catalog.Orders.Queries.GetOrdersById;
using Snagged.Application.Catalog.Orders.Queries.GetOrdersPaged;
using Snagged.Application.Common.Paging;

namespace Snagged.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<OrderDto>>> GetOrders([FromQuery] GetOrdersQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetOrderById(int id)
        {
            try
            {
                var query = new GetOrdersByIdQuery { Id = id };
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Order with Id {id} not found.");
            }
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateOrder([FromBody] CreateOrderCommand command)
        {
            var orderId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetOrderById), new { id = orderId }, orderId);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] UpdateOrderCommand command)
        {
            if (id != command.Id)
                return BadRequest("ID mismatch");

            try
            {
                await _mediator.Send(command);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Order with Id {id} not found.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            try
            {
                var command = new DeleteOrderCommand { Id = id };
                await _mediator.Send(command);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Order with Id {id} not found.");
            }
        }

        [HttpGet("paged")]
        public async Task<ActionResult<PageResult<OrderDto>>> GetPagedOrders([FromQuery] GetOrdersPagedQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
