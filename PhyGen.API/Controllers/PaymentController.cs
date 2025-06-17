using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Net.payOS.Types;
using PhyGen.Application.Authentication.Interface;
using PhyGen.Application.Authentication.Models.Requests;
using PhyGen.Application.Notification.Commands;
using PhyGen.Application.PayOs.Interfaces;
using PhyGen.Application.PayOs.Request;
using PhyGen.Application.Users.Exceptions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PhyGen.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("create-payment")]
        [Authorize]
        public async Task<IActionResult> CreatePayment([FromBody] PaymentRequest request)
        {
            var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                    ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Chuyển đổi userId sang Guid
            if (!Guid.TryParse(userId, out var parsedUserId))
            {
                return BadRequest("UserId không hợp lệ.");
            }

            // Gán userId vào request
            request.UserId = parsedUserId;
            var response = await _paymentService.CreatePaymentLinkAsync(request);
            return Ok(response);
        }

        [HttpGet("status/{paymentLinkId}")]
        public async Task<IActionResult> GetPaymentStatus(long paymentLinkId)
        {
            var payment = await _paymentService.GetPaymentStatusAsync(paymentLinkId);
            return Ok(payment);
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> HandleWebhook([FromBody] WebhookRequest request)
        {
            var result = await _paymentService.HandleWebhookByTransactionIdAsync(request.TransactionId.ToString());

            if (result.Success)
                return Ok(result.Message);

            return BadRequest(result.Message);
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search(
        [FromQuery] Guid UserId,
        [FromQuery] DateTime? FromDate,
        [FromQuery] DateTime? ToDate,
        [FromQuery] string? Status)
        {
            var request = new PaymentSearchRequest
            {
                UserId = UserId,
                FromDate = FromDate,
                ToDate = ToDate,
                Status = Status
            };
            var result = await _paymentService.SearchPaymentsAsync(request);
            return Ok(result);
        }
    }
}
