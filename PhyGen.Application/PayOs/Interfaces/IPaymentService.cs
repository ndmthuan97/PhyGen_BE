using Net.payOS.Types;
using PhyGen.Application.PayOs.Request;
using PhyGen.Application.PayOs.Response;
using PhyGen.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.PayOs.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentResponse> CreatePaymentLinkAsync(PaymentRequest request);
        Task<Payments> GetPaymentStatusAsync(long paymentLinkId);
        Task<WebhookResult> HandleWebhookByTransactionIdAsync(string value);
        Task<List<SearchPaymentResponse>> SearchPaymentsAsync(PaymentSearchRequest request);

    }
}
