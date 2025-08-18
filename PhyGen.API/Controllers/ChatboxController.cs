using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using PhyGen.API.Controllers;
using PhyGen.Application.Authentication.DTOs.Dtos;
using PhyGen.Application.Authentication.DTOs.Responses;
using PhyGen.Application.Authentication.Interface;
using PhyGen.Application.Authentication.Models.Requests;
using PhyGen.Application.Authentication.Responses;
using PhyGen.Infrastructure.Service;
using PhyGen.Shared.Constants;
using System.Security.Claims;
using static PhyGen.Infrastructure.Service.GeminiService;


namespace PhyGen.API.Controllers
{
    [ApiController]
    [Route("api/chatbox")]

    public class ChatboxController : ControllerBase
    {
        private readonly GeminiService _geminiService;
        private readonly IMemoryCache _memoryCache;

        public ChatboxController(GeminiService geminiService, IMemoryCache memoryCache)
        {
            _geminiService = geminiService;
            _memoryCache = memoryCache;
        }

        private const int CacheMinutes = 120;

        private List<GeminiService.MessageRequest> GetOrCreateHistory(string userId)
        {
            var sessionKey = $"chat_history_{userId}";
            return _memoryCache.GetOrCreate(sessionKey, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(CacheMinutes);
                return new List<GeminiService.MessageRequest>();
            });
        }

        private void TouchHistory(string userId, List<GeminiService.MessageRequest> history)
        {
            var sessionKey = $"chat_history_{userId}";
            _memoryCache.Set(sessionKey, history, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(CacheMinutes)
            });
        }

        private string GetUserId()
        {
            var id = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                  ?? User.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(id))
                throw new UnauthorizedAccessException("Không tìm thấy userId trong token.");

            return id;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Chat([FromBody] GeminiService.MessageRequest newMessage)
        {
            var userId = GetUserId();
            var history = GetOrCreateHistory(userId);

            // client message
            newMessage.Timestamp = DateTime.UtcNow;
            history.Add(newMessage);

            // model response
            var response = await _geminiService.CallGeminiAsync(history);
            response.Timestamp = DateTime.UtcNow;
            history.Add(response);

            // gia hạn TTL
            TouchHistory(userId, history);

            return Ok(new { userId, history });
        }

        // GET: api/chat/history  (không cần truyền gì)
        [Authorize]
        [HttpGet("history")]
        public IActionResult GetHistory([FromQuery] int? take) // hoặc bỏ luôn param này
        {
            var userId = GetUserId();
            var sessionKey = $"chat_history_{userId}";

            if (!_memoryCache.TryGetValue(sessionKey, out List<GeminiService.MessageRequest> history) || history == null)
            {
                // Không có lịch sử => rỗng
                return Ok(new { userId, history = new List<GeminiService.MessageRequest>() });
            }

            IEnumerable<GeminiService.MessageRequest> result = history;

            if (take.HasValue && take.Value > 0)
            {
                result = history
                    .OrderByDescending(m => m.Timestamp)
                    .Take(take.Value)
                    .OrderBy(m => m.Timestamp);
            }

            // gia hạn TTL mỗi lần GET
            TouchHistory(userId, history);

            return Ok(new { userId, history = result.ToList() });
        }
    }
}