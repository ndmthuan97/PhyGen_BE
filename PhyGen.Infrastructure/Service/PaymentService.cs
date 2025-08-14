using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Net.payOS;
using Net.payOS.Types;
using PhyGen.Application.Notification.Commands;
using PhyGen.Application.PayOs.Config;
using PhyGen.Application.PayOs.Interfaces;
using PhyGen.Application.PayOs.Request;
using PhyGen.Application.PayOs.Response;
using PhyGen.Application.Users.Exceptions;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Specs;
using PhyGen.Infrastructure.Persistence.DbContexts;
using PhyGen.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StackExchange.Redis.Role;

namespace PhyGen.Infrastructure.Service
{
    public class PaymentService : IPaymentService
    {

        private readonly PayOS _payOS;
        private readonly AppDbContext _context;
        private readonly PayOSConfig _config;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public PaymentService(IOptions<PayOSConfig> config, AppDbContext context, IMapper mapper, IMediator mediator)
        {
            _config = config.Value;
            _payOS = new PayOS(_config.ClientId, _config.ApiKey, _config.ChecksumKey);
            _context = context;
            _mapper = mapper;
            _mediator = mediator;
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
                description: $"Thanh toán {user.UserCode}",
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
                Amount = request.Amount * 1000,
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

        public async Task<Payment> GetPaymentStatusAsync(long paymentLinkId)
        {
            var payment = await _context.Payments
                .FirstOrDefaultAsync(p => p.PaymentLinkId == paymentLinkId);

            if (payment == null)
            {
                throw new Exception("Không tìm thấy giao dịch.");
            }

            // Lấy thông tin từ PayOS
            PaymentLinkInformation info = await _payOS.getPaymentLinkInformation(payment.PaymentLinkId);
            payment.Status = (payment.ValidUntil < DateTime.UtcNow && info.status != "PAID")
                            ? PaymentStatus.Expired.ToString()
                            : info.status == "PAID" ? PaymentStatus.Completed.ToString()
                            : info.status == "CANCELLED" ? PaymentStatus.Cancelled.ToString()
                            : PaymentStatus.Pending.ToString();
            await _context.SaveChangesAsync();
            return payment;
        }

        public async Task<WebhookResult> HandleWebhookByTransactionIdAsync(long paymentLinkId)
        {
            try
            {
                // 1. Tìm payment theo paymenlinkId
                var payment = await _context.Payments
                    .FirstOrDefaultAsync(p => p.PaymentLinkId == paymentLinkId);

                if (payment == null)
                {
                    string msg = $"[Webhook] Không tìm thấy giao dịch với orderCode: {paymentLinkId}";
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
                    nameof(PaymentStatus.Cancelled) => "Giao dịch đã bị hủy.",
                    nameof(PaymentStatus.Expired) => "Giao dịch đã hết hạn",
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

                        var transaction = new PhyGen.Domain.Entities.Transaction
                        {
                            Id = Guid.NewGuid(),
                            UserId = user.Id,
                            CoinAfter = user.Coin,
                            CoinBefore = user.Coin - coinsToAdd,
                            CoinChange = +coinsToAdd,
                            TypeChange = "PayOS",
                            PaymentlinkID = payment.PaymentLinkId,
                            CreatedAt = DateTime.UtcNow
                        };
                        _context.Transactions.Add(transaction);
                        await _context.SaveChangesAsync();
               
                        // Gửi thông báo: giao dịch thành công
                        await _mediator.Send(new CreateNotificationCommand
                        {
                            UserId = user.Id,
                            Title = "Giao dịch thành công",
                            Message = $"Bạn đã thanh toán thành công {payment.Amount} xu. Cảm ơn bạn!",
                            CreatedAt = DateTime.UtcNow
                        });
                    }
                    else
                    {
                        throw new UserNotFoundException();
                    }
                }
                else if (payment.Status == PaymentStatus.Cancelled.ToString())
                {
                    // Gửi thông báo: giao dịch bị hủy
                    await _mediator.Send(new CreateNotificationCommand
                    {
                        UserId = payment.UserId,
                        Title = "Giao dịch bị hủy",
                        Message = "Giao dịch của bạn đã bị hủy. Vui lòng thử lại.",
                        CreatedAt = DateTime.UtcNow
                    });
                }
                else if (payment.Status == PaymentStatus.Expired.ToString())
                {
                    await _mediator.Send(new CreateNotificationCommand
                    {
                        UserId = payment.UserId,
                        Title = "Giao dịch hết hạn",
                        Message = "Liên kết thanh toán đã hết hạn. Vui lòng tạo lại để tiếp tục.",
                        CreatedAt = DateTime.UtcNow
                    });
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

        public async Task<Pagination<SearchTransactionResponse>> SearchTransactionAsync(SearchTransactionRequest request)
        {
            var query = _context.Transactions.AsQueryable();

            query = query.Where(p => p.UserId == request.UserId);

            if (request.FromDate.HasValue)
                query = query.Where(p => p.CreatedAt >= request.FromDate.Value);

            if (request.ToDate.HasValue)
                query = query.Where(p => p.CreatedAt <= request.ToDate.Value);

            if (!string.IsNullOrWhiteSpace(request.Type))
            {
                var typeNormalized = request.Type.Trim().ToUpperInvariant();
                query = query.Where(p => p.TypeChange.ToUpper() == typeNormalized);
            }

            query = query.OrderByDescending(p => p.CreatedAt);

            var totalItems = await query.CountAsync();

            var items = await query
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            var data = _mapper.Map<List<SearchTransactionResponse>>(items);

            return new Pagination<SearchTransactionResponse>(
                request.PageIndex,
                request.PageSize,
                totalItems,
                data
            );
        }

    }
}
