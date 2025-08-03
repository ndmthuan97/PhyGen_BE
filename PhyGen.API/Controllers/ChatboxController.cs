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

        [Authorize] 
        [HttpPost("chatbox")]
        public async Task<IActionResult> Chat([FromBody] GeminiService.MessageRequest newMessage)
        {
            string userId = GetUserId();
            string sessionKey = $"chat_history_{userId}";

            // Lấy lịch sử từ MemoryCache
            var history = _memoryCache.GetOrCreate(sessionKey, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(120);
                return new List<GeminiService.MessageRequest>();
            });

            // Ghi lại thời gian người dùng gửi câu hỏi
            newMessage.Timestamp = DateTime.UtcNow;
            history.Add(newMessage);

            // Gọi Gemini và nhận phản hồi
            var response = await _geminiService.CallGeminiAsync(history);
            response.Timestamp = DateTime.UtcNow;
            history.Add(response);

            // Lưu lại vào cache
            _memoryCache.Set(sessionKey, history);

            return Ok(new
            {
                userId,
                history
            });
        }

        private string GetUserId()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedAccessException("Không tìm thấy userId trong token.");
            }

            return userId;
        }
    }

}