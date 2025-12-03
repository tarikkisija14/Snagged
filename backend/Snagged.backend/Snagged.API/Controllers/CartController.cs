using MediatR;
using Microsoft.AspNetCore.Mvc;
using Snagged.Application.Catalog.Cart;
using Snagged.Application.Catalog.Cart.Commands.AddCartItem;
using Snagged.Application.Catalog.Cart.Commands.Checkout;
using Snagged.Application.Catalog.Cart.Commands.ClearCart;
using Snagged.Application.Catalog.Cart.Commands.DeleteCartItem;
using Snagged.Application.Catalog.Cart.Commands.UpdateCartItem;
using Snagged.Application.Catalog.Cart.Queries.GetAllCarts;
using Snagged.Application.Catalog.Cart.Queries.GetCartByUser;

namespace Snagged.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CartController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<CartDto>>> GetAllCarts()
        {
            var result = await _mediator.Send(new GetAllCartsQuery());
            return Ok(result);
        }


        [HttpGet("user/{userId}")]
        public async Task<ActionResult<CartDto>> GetCartByUserId(int userId)
        {
            var result = await _mediator.Send(new GetCartByUserQuery { UserId = userId });
            if (result == null)
                return NotFound();

            return Ok(result);
        }
        [HttpPost("item")]
        public async Task<ActionResult<int>> AddCartItem([FromBody] AddCartItemCommand command)
        {
            var cartId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetCartByUserId), new { userId = command.UserId }, cartId);
        }

        [HttpPut("item/{cartItemId}")]
        public async Task<IActionResult> UpdateCartItem(int cartItemId, [FromBody] UpdateCartitemCommand command)
        {
            if (cartItemId != command.CartItemId)
                return BadRequest("CartItemId mismatch");

            await _mediator.Send(command);
            return NoContent();
        }
        [HttpDelete("item/{cartItemId}")]
        public async Task<IActionResult> DeleteCartItem(int cartItemId)
        {
            await _mediator.Send(new DeleteCartItemCommand { CartItemId = cartItemId });
            return NoContent();
        }
        [HttpDelete("user/{userId}/clear")]
        public async Task<IActionResult> ClearCart(int userId)
        {
            await _mediator.Send(new ClearCartCommand { UserId = userId });
            return NoContent();
        }
        [HttpPost("checkout")]
        public async Task<ActionResult<int>> Checkout([FromBody] CheckoutCommand command)
        {
            var orderId = await _mediator.Send(command);
            return Ok(orderId); 
        }


    }
}
