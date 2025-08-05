using Microsoft.Extensions.Configuration;
using PhyGen.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PhyGen.Infrastructure.Service
{
    public class GeminiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiKey;
        public GeminiService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _apiKey = configuration["GeminiApi:ApiKey"];
        }

        public async Task<MessageRequest> CallGeminiAsync(List<MessageRequest> messages)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var requestUri = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={_apiKey}";

            var contents = messages.Select(msg => new
            {
                role = msg.Role,
                parts = new[] { new { text = msg.Text } }
            });

            var payload = new { contents };

            var response = await httpClient.PostAsJsonAsync(requestUri, payload);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return new MessageRequest
                {
                    Role = "model",
                    Text = $"Lỗi Gemini API: {response.StatusCode}\nChi tiết: {error}"
                };
            }

            var result = await response.Content.ReadFromJsonAsync<GeminiResponse>();
            var text = result?.candidates?[0]?.content?.parts?[0]?.text ?? "Không có phản hồi từ Gemini.";

            return new MessageRequest
            {
                Role = "model",
                Text = text
            };
        }

        public class MessageRequest
        {
            public DateTime Timestamp { get; set; } = DateTime.UtcNow; // thời gian tin nhắn
            public string Role { get; set; } // "user" hoặc "model"
            public string Text { get; set; } // nội dung tin nhắn
        }

        public class GeminiResponse
        {
            public List<Candidate> candidates { get; set; }

            public class Candidate
            {
                public Content content { get; set; }
            }

            public class Content
            {
                public List<Part> parts { get; set; }
            }

            public class Part
            {
                public string text { get; set; }
            }
        }
    }
}
