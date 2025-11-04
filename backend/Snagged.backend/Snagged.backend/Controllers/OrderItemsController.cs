using MediatR;
using Microsoft.AspNetCore.Mvc;
using Snagged.Application.Catalog.OrderItem;
using Snagged.Application.Catalog.OrderItem.Commands.CreateOrderItem;
using Snagged.Application.Catalog.OrderItem.Commands.DeleteOrderItem;
using Snagged.Application.Catalog.OrderItem.Commands.UpdateOrderItem;
using Snagged.Application.Catalog.OrderItem.Queries.GetOrderItemById;
using Snagged.Application.Catalog.OrderItem.Queries.GetOrderItems;
using Snagged.Application.Catalog.OrderItem.Queries.GetOrderItemsByOrderId;
using Snagged.Application.Catalog.Orders.Commands;

namespace Snagged.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderItemsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderItemsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<OrderItemDto>>> GetAllOrderItems()
        {
            var result = await _mediator.Send(new GetOrderItemsQuery());
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateOrderItem([FromBody] CreateOrderItemCommand command)
        {
            try
            {
                var id = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetOrderItemById), new { id }, id);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("item/{id}")]
        public async Task<ActionResult<OrderItemDto>> GetOrderItemById(int id)
        {
            try
            {
                var result = await _mediator.Send(new GetOrderItemByIdQuery { Id = id });
                if (result == null)
                    return NotFound();

                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrderItem(int id, [FromBody] UpdateOrderItemCommand command)
        {
            if (id != command.Id)
                return BadRequest("Id mismatch");

            try
            {
                await _mediator.Send(command);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderItem(int id)
        {
            try
            {
                await _mediator.Send(new DeleteOrderItemCommand { Id = id });
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("order/{orderId}")]
        public async Task<ActionResult<List<OrderItemDto>>> GetByOrderId(int orderId)
        {
            var result = await _mediator.Send(new GetOrderItemsByOrderIdQuery { OrderId = orderId });
            return Ok(result);
        }
    }
}
