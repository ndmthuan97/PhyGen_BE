using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PhyGen.Application.Questions.Dtos;
using PhyGen.Application.Topics.Queries;
using PhyGen.Domain.Specs.Topic;
using PhyGen.Infrastructure.Persistence.DbContexts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PhyGen.Infrastructure.Service
{
    public class ChatGptService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiKey;
        private readonly AppDbContext _dbContext;
        private readonly IMediator _mediator;

        // Inject thêm IConfiguration
        public ChatGptService(IHttpClientFactory httpClientFactory, IConfiguration configuration, AppDbContext dbContext, IMediator mediator)
        {
            _httpClientFactory = httpClientFactory;
            _dbContext = dbContext;
            _mediator = mediator;
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

        public async Task<List<ExtractedQuestionDto>> ExtractQuestionsFromTextAsync(string questionContent, string fileName, int grade)
        {
            var param = new TopicByGradeSpecParam { Grade = grade };
            var query = new GetTopicsByGradeQuery(param);
            var topicResponses = await _mediator.Send(query);
            var topicNames = topicResponses.Select(x => x.Name).ToList();

            string prompt = $@"Hãy đóng vai giáo viên vật lý, lập trình viên
            Đây là nội dung đề kiểm tra vật lý lấy từ file {fileName}:
            ---
            {questionContent}
            ---
            - Hãy tách riêng từng câu hỏi trong đề (bỏ số thứ tự).
            - Đối với mỗi câu hỏi, trả về dạng JSON:
              {{
                ""Content"": Nội dung câu hỏi, không chứa các đáp án trừ loại TrueFalse
                ""Type"": Loại câu hỏi (MultipleChoice/TrueFalse/ShortAnswer/Essay),
                ""Level"": ""Easy"" hoặc ""Medium"" hoặc ""Hard"" (chính xác, không dấu cách, không tiếng Việt, không viết hoa hết, không thừa khoảng trắng)
                ""TopicName"": Tên chủ đề (chọn từ danh sách sao cho phù hợp với câu hỏi từ list chủ đề :{string.Join(", ", topicNames)}), chỉ chọn 1 và không thay đổi nội dung chủ đề,
                ""Answer1"": ..., ""Answer2"": ..., và các câu trả lời khác tùy từng câu
                ""Grade"": Lớp, //Grade phải là số nguyên. Không để trong dấu nháy
              }}
            - Trả về danh sách [ {{...}}, ... ].
            - Phần MultipleChoice trả ra các đáp án ko cần A., B., C., D. ở đầu
            - Phần TrueFalse/ShortAnswer/Essay lưu hết vào content, phần Answer để null (Phần TrueFalse 1 câu chỉ trả ra 1 lần)
            - Câu nào có công thức giúp tôi viết bằng LaTex theo thư viện better-react-mathjax để hiển thị cho FE có thể hanlde không bị lỗi (trả ra theo dạng $...$)
            - **QUAN TRỌNG: trong JSON, mọi ký tự '\' phải được ghi thành '\\\\'. Ví dụ: \alpha -> \\\\alpha, \frac -> \\\\frac**, trả về JSON để ko bị lỗi khi parse bằng System.Text.Json ngôn ngữ C# ASP.NET
            - Không trả lại bất cứ văn bản hay giải thích nào ngoài JSON!
            ";

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
                throw new Exception("OpenAI API call failed: " + error);
            }

            var result = await response.Content.ReadFromJsonAsync<ChatGptResponse>();
            var resultText = result?.choices?.FirstOrDefault()?.message?.content;
            if (string.IsNullOrWhiteSpace(resultText))
                throw new Exception("Không nhận được dữ liệu từ ChatGPT!");

            // Lọc code block nếu có
            var jsonStart = resultText.IndexOf('[');
            var jsonEnd = resultText.LastIndexOf(']');
            if (jsonStart >= 0 && jsonEnd > jsonStart)
                resultText = resultText.Substring(jsonStart, jsonEnd - jsonStart + 1);

            var questions = JsonSerializer.Deserialize<List<ExtractedQuestionDto>>(resultText, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter() }
            });

            return questions ?? new List<ExtractedQuestionDto>();
        }

        public async Task<bool> IsPhysicsExamAsync(string fileName, string fullText, CancellationToken ct = default)
        {
            // Prompt rất ngắn, chỉ yêu cầu 1 object JSON có đúng 1 khóa isPhysicsExam
            var prompt = $@"
            Bạn là giáo viên Vật lý. Hãy đọc nội dung sau và CHỈ trả về một JSON object hợp lệ có duy nhất khóa:
            - isPhysicsExam: boolean

            Định nghĩa: isPhysicsExam=true nếu nội dung chủ yếu là các câu hỏi/đề mục thuộc môn Vật lý (cơ học, nhiệt học, điện, quang, hạt nhân, v.v.), có cấu trúc đề (nhiều câu hỏi, A/B/C/D, đúng-sai, tự luận, công thức/ký hiệu vật lý, đơn vị SI)
            Chỉ có list câu hỏi vật lý mới được coi là danh sách câu hỏi Vật lý, những thứ khác liên quan tới Vật lý nhưng không phải đề (ví dụ:lý thuyết, công thức, ma trận) thì không được coi là đề Vật lý thì sẽ trả về false.

            Ví dụ đầu ra hợp lệ:
            {{""isPhysicsExam"": true}}

            Nếu không phải đề Vật lý:
            {{""isPhysicsExam"": false}}

            Nội dung cần kiểm tra:
            ---
            FileName: {fileName}
            {fullText}
            ---";

            var http = _httpClientFactory.CreateClient();
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

            // Dùng response_format=json_object để buộc trả JSON object
            var payload = new
            {
                model = "gpt-4.1",
                response_format = new { type = "json_object" },
                messages = new[]
                {
                new { role = "system", content = "Chỉ trả về JSON object hợp lệ. Không thêm văn bản khác." },
                new { role = "user", content = prompt }
            }
            };

            using var req = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions")
            {
                Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json")
            };

            using var resp = await http.SendAsync(req, ct);
            if (!resp.IsSuccessStatusCode)
            {
                // Mọi lỗi coi như không phải đề Vật lý
                return false;
            }

            // Đọc raw text rồi parse JSON object (response_format đảm bảo là JSON)
            var raw = await resp.Content.ReadAsStringAsync(ct);
            try
            {
                using var doc = JsonDocument.Parse(raw);
                var content = doc.RootElement
                    .GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString();

                if (string.IsNullOrWhiteSpace(content)) return false;

                using var doc2 = JsonDocument.Parse(content);
                if (doc2.RootElement.TryGetProperty("isPhysicsExam", out var prop))
                {
                    // chấp nhận true/false; mọi trường hợp khác -> false
                    return prop.ValueKind == JsonValueKind.True
                        ? true
                        : prop.ValueKind == JsonValueKind.False ? false : false;
                }
            }
            catch
            {
                // Nếu parse lỗi → fail-safe
                return false;
            }

            return false;
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
