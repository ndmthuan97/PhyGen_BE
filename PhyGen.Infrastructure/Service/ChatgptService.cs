using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace PhyGen.Infrastructure.Service
{
    public class ChatGptService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiKey;

        // Inject thêm IConfiguration
        public ChatGptService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            // Đọc key từ cấu hình
            _apiKey = configuration["OpenAI:ApiKey"];
        }

        public async Task<string> CallChatGptAsync(string prompt)
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _apiKey);

            var payload = new
            {
                model = "gpt-4.1",
                messages = new[]
                {
                    new { role = "system", content = "Bạn là giáo viên vật lý." },
                    new { role = "user", content = prompt }
                }
            };

            var response = await httpClient.PostAsJsonAsync("https://api.openai.com/v1/chat/completions", payload);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine("OpenAI API Error: " + error);
                return null;
            }

            var result = await response.Content.ReadFromJsonAsync<ChatGptResponse>();
            return result?.choices?.FirstOrDefault()?.message?.content;
        }

        public class ChatGptResponse
        {
            public List<Choice> choices { get; set; }
            public class Choice
            {
                public Message message { get; set; }
            }
            public class Message
            {
                public string content { get; set; }
            }
        }
    }
}
