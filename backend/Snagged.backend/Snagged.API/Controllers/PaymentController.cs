using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snagged.Application.Catalog.Payment;
using Snagged.Application.Catalog.Payment.Commands.CreatePayment;
using Snagged.Application.Catalog.Payment.Commands.CreateStripePayment;
using Snagged.Application.Catalog.Payment.Queries.GetAllPayments;
using Snagged.Application.Catalog.Payment.Queries.GetAllPaymentsByUser;
using Snagged.Application.Common.Exceptions;

namespace Snagged.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PaymentController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<PaymentDto>>> GetAllPayments()
        {
            var result = await mediator.Send(new GetAllPaymentsQuery());
            return Ok(result);
        }

        [HttpGet("user/{userId:int}")]
        public async Task<ActionResult<List<PaymentDto>>> GetPaymentsByUser(int userId)
        {
            var result = await mediator.Send(new GetPaymentsByUserQuery { UserId = userId });
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreatePayment([FromBody] CreatePaymentCommand command)
        {
            try
            {
                var paymentId = await mediator.Send(command);
                return Ok(new { paymentId });
            }
            catch (SnaggedNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpPost("stripe/create-intent/{orderId:int}")]
        public async Task<ActionResult> CreateStripePaymentIntent(int orderId)
        {
            try
            {
                var clientSecret = await mediator.Send(new CreateStripePaymentCommand
                {
                    OrderId = orderId,
                    Currency = "usd"
                });

                return Ok(new { clientSecret });
            }
            catch (SnaggedNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}