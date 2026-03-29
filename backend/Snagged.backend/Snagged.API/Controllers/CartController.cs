using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snagged.Application.Catalog.Cart.Commands.AddCartItem;
using Snagged.Application.Catalog.Cart.Commands.Checkout;
using Snagged.Application.Catalog.Cart.Commands.ClearCart;
using Snagged.Application.Catalog.Cart.Commands.DeleteCartItem;
using Snagged.Application.Catalog.Cart.Commands.MoveToCart;
using Snagged.Application.Catalog.Cart.Commands.SaveForLater;
using Snagged.Application.Catalog.Cart.Commands.UpdateCartItem;
using Snagged.Application.Catalog.Cart.Queries.GetAllCarts;
using Snagged.Application.Catalog.Cart.Queries.GetCartByUser;
using Snagged.Application.Catalog.Cart.Queries.GetSavedCartByUser;
using Snagged.Application.Common.Exceptions;

namespace Snagged.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CartController(IMediator mediator) : ControllerBase
    {
        [HttpGet("all")]
        public async Task<IActionResult> GetAllCarts()
        {
            var result = await mediator.Send(new GetAllCartsQuery());
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetMyCart()
        {
            var result = await mediator.Send(new GetCartByUserQuery());
            if (result is null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("saved")]
        public async Task<IActionResult> GetSavedCart()
        {
            var result = await mediator.Send(new GetSavedCartByUserQuery());
            return Ok(result);
        }

        [HttpPost("item")]
        public async Task<IActionResult> AddCartItem([FromBody] AddCartItemCommand command)
        {
            try
            {
                var cartId = await mediator.Send(command);
                return CreatedAtAction(nameof(GetMyCart), new { }, new { cartId });
            }
            catch (SnaggedNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpPut("item/{cartItemId:int}")]
        public async Task<IActionResult> UpdateCartItem(
            int cartItemId, [FromBody] UpdateCartitemCommand command)
        {
            if (cartItemId != command.CartItemId)
                return BadRequest(new { error = "CartItemId in URL does not match the request body." });

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

        [HttpDelete("item/{cartItemId:int}")]
        public async Task<IActionResult> DeleteCartItem(int cartItemId)
        {
            try
            {
                await mediator.Send(new DeleteCartItemCommand { CartItemId = cartItemId });
                return NoContent();
            }
            catch (SnaggedNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpDelete("clear")]
        public async Task<IActionResult> ClearCart()
        {
            await mediator.Send(new ClearCartCommand());
            return NoContent();
        }

        [HttpPut("save-for-later/{cartId:int}")]
        public async Task<IActionResult> SaveForLater(int cartId)
        {
            await mediator.Send(new SaveForLaterCommand { CartId = cartId });
            return NoContent();
        }

        [HttpPut("move-to-cart/{cartId:int}")]
        public async Task<IActionResult> MoveToCart(int cartId)
        {
            await mediator.Send(new MoveToCartCommand { CartId = cartId });
            return NoContent();
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout([FromBody] CheckoutCommand command)
        {
            try
            {
                var orderId = await mediator.Send(command);
                return Ok(new { orderId });
            }
            catch (SnaggedNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (SnaggedBusinessRuleException ex)
            {
                return Conflict(new { error = ex.Message });
            }
        }
    }
}