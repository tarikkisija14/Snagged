using MediatR;
using Microsoft.AspNetCore.Mvc;
using Snagged.Application.Commom.Paging;
using Snagged.Application.Orders.Commands;
using Snagged.Application.Orders.Commands.CreateOrder;
using Snagged.Application.Orders.Commands.DeleteOrder;
using Snagged.Application.Orders.Commands.UpdateOrder;
using Snagged.Application.Orders.Queries.GetOrders;
using Snagged.Application.Orders.Queries.GetOrdersById;

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
            var query = new GetOrdersByIdQuery { Id = id };
            var result = await _mediator.Send(query);
            return Ok(result);
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

            await _mediator.Send(command);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var command = new DeleteOrderCommand { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }
        [HttpGet("paged")]
        public async Task<ActionResult<PageResult<OrderDto>>> GetPagedOrders([FromQuery] GetOrdersPagedQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

    }
}
