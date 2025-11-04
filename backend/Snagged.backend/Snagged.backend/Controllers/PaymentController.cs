using MediatR;
using Microsoft.AspNetCore.Mvc;
using Snagged.Application.Catalog.Payment;
using Snagged.Application.Catalog.Payment.Commands.CreatePayment;
using Snagged.Application.Catalog.Payment.Queries.GetAllPayments;
using Snagged.Application.Catalog.Payment.Queries.GetAllPaymentsByUser;

namespace Snagged.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaymentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<PaymentDto>>> GetAllPayments()
        {
            var result = await _mediator.Send(new GetAllPaymentsQuery());
            return Ok(result);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<PaymentDto>>> GetPaymentsByUser(int userId)
        {
            var result = await _mediator.Send(new GetPaymentsByUserQuery { UserId = userId });
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreatePayment([FromBody] CreatePaymentCommand command)
        {
            var paymentId = await _mediator.Send(command);
            return Ok(paymentId);
        }
    }
}
