using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Net.payOS;
using Net.payOS.Types;
using PhyGen.Application.PayOs.Config;
using PhyGen.Application.PayOs.Interfaces;
using PhyGen.Application.PayOs.Request;
using PhyGen.Application.PayOs.Response;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Specs;
using PhyGen.Insfrastructure.Persistence.DbContexts;
using PhyGen.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StackExchange.Redis.Role;

namespace PhyGen.Insfrastructure.Service
{
    public class PaymentService : IPaymentService
    {

        private readonly PayOS _payOS;
        private readonly AppDbContext _context;
        private readonly PayOSConfig _config;
        private readonly IMapper _mapper;

        public PaymentService(IOptions<PayOSConfig> config, AppDbContext context, IMapper mapper)
        {
            _config = config.Value;
            _payOS = new PayOS(_config.ClientId, _config.ApiKey, _config.ChecksumKey);
            _context = context;
            _mapper = mapper;

        }

        public async Task<PaymentResponse> CreatePaymentLinkAsync(PaymentRequest request)
        {
            // Kiểm tra người dùng tồn tại
            var user = await _context.Users.FindAsync(request.UserId);
            if (user == null)
            {
                throw new ArgumentException($"Người dùng với UserId {request.UserId} không tồn tại.");
            }
            if (request.Amount <= 0)
            {
                throw new ArgumentException("Số tiền phải lớn hơn 0.");
            }

            // Unique code for each order
            var orderCode = (long)(DateTime.UtcNow.Ticks / TimeSpan.TicksPerSecond);

            var items = new List<ItemData>
            {
                new ItemData($"Subscription MailMate", 1, (int)(request.Amount * 1000)) // PayOS yêu cầu số tiền tính bằng VNĐ
            };

            // Tạo PaymentData
            var paymentData = new PaymentData(
                orderCode: orderCode,
                amount: (int)(request.Amount * 1000), // Chuyển đổi sang VNĐ
                description: $"Thanh toán",
                items: items,
                returnUrl: request.ReturnUrl,
                cancelUrl: request.CancelUrl
            );

            // Gọi PayOS để tạo link thanh toán
            CreatePaymentResult result = await _payOS.createPaymentLink(paymentData);

            // Lưu thông tin thanh toán vào cơ sở dữ liệu
            var payment = new Payment
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                Amount = request.Amount,
                PaymentLinkId = result.orderCode,
                Status = PaymentStatus.Pending.ToString(),
                CreatedAt = DateTime.UtcNow,
                ValidUntil = DateTime.UtcNow.AddMinutes(30)
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            return new PaymentResponse
            {
                CheckoutUrl = result.checkoutUrl,
                PaymentLinkId = result.orderCode,
                Status = PaymentStatus.Pending
            };
        }

        public async Task<Payment> GetPaymentStatusAsync(long paymenLinkId)
        {
            var payment = await _context.Payments
                .FirstOrDefaultAsync(p => p.PaymentLinkId == paymenLinkId);

            if (payment == null)
            {
                throw new Exception("Không tìm thấy giao dịch.");
            }

            // Lấy thông tin từ PayOS
            PaymentLinkInformation info = await _payOS.getPaymentLinkInformation(payment.PaymentLinkId);
            payment.Status = info.status == "PAID" ? PaymentStatus.Completed.ToString() :
                             info.status == "CANCELLED" ? PaymentStatus.Failed.ToString() : PaymentStatus.Pending.ToString();

            await _context.SaveChangesAsync();
            return payment;
        }

        public async Task<WebhookResult> HandleWebhookByTransactionIdAsync(long orderCode)
        {
            try
            {
                // 1. Tìm payment theo paymenlinkId
                var payment = await _context.Payments
                    .FirstOrDefaultAsync(p => p.PaymentLinkId == orderCode);

                if (payment == null)
                {
                    string msg = $"[Webhook] Không tìm thấy giao dịch với orderCode: {orderCode}";
                    Console.WriteLine(msg);
                    return new WebhookResult { Success = false, Message = msg };
                }

                // 2. Gọi lại method để cập nhật trạng thái từ PayOS
                payment = await GetPaymentStatusAsync(payment.PaymentLinkId);

                // 3. Tùy theo trạng thái, xử lý tương ứng
                string statusMessage = payment.Status switch
                {
                    nameof(PaymentStatus.Completed) => "Xử lý giao dịch thành công.",
                    nameof(PaymentStatus.Pending) => "Giao dịch đang chờ xử lý.",
                    nameof(PaymentStatus.Failed) => "Giao dịch đã bị hủy.",
                    _ => "Trạng thái giao dịch không xác định."
                };

                // 4. Nếu đã thanh toán thành công => cộng xu
                if (payment.Status == PaymentStatus.Completed.ToString())
                {
                    var user = await _context.Users.FindAsync(payment.UserId);
                    if (user != null)
                    {
                        int coinsToAdd = (int)payment.Amount;
                        user.Coin += coinsToAdd;

                        Console.WriteLine($"[Webhook] Cộng {coinsToAdd} xu cho user {user.Id} từ giao dịch {payment.PaymentLinkId}");
                        await _context.SaveChangesAsync();
                    }
                }

                Console.WriteLine($"[Webhook] Xử lý hoàn tất cho giao dịch {payment.PaymentLinkId}, trạng thái: {payment.Status}");

                return new WebhookResult
                {
                    Success = true,
                    Message = statusMessage
                };
            }
            catch (Exception ex)
            {
                string err = $"[Webhook][Error] {ex.Message}";
                Console.WriteLine(err);
                return new WebhookResult
                {
                    Success = false,
                    Message = err
                };
            }
        }
        public async Task<Pagination<SearchPaymentResponse>> SearchPaymentsAsync(PaymentSearchRequest request)
        {
            var query = _context.Payments.AsQueryable();

            query = query.Where(p => p.UserId == request.UserId);

            if (request.FromDate.HasValue)
                query = query.Where(p => p.CreatedAt >= request.FromDate.Value);

            if (request.ToDate.HasValue)
                query = query.Where(p => p.CreatedAt <= request.ToDate.Value);

            if (!string.IsNullOrWhiteSpace(request.Status))
            {
                var statusNormalized = request.Status.Trim().ToUpperInvariant();
                query = query.Where(p => p.Status.ToUpper() == statusNormalized);
            }

            query = query.OrderByDescending(p => p.CreatedAt);

            var totalItems = await query.CountAsync();

            var items = await query
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            var data = _mapper.Map<List<SearchPaymentResponse>>(items);

            return new Pagination<SearchPaymentResponse>(
                request.PageIndex,
                request.PageSize,
                totalItems,
                data
            );
        }
    }
}
