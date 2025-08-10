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
        public async Task<string> GenerateManimCodeFromPrompt(string imagePrompt)
        {
            string prompt = $@"
                Bạn là chuyên gia Python, vẽ vật lý bằng manim.
                Viết code Python dùng thư viện manim để vẽ hình minh họa cho mô tả sau: {imagePrompt}
                Yêu cầu:
                - Một class duy nhất kế thừa Scene, tên tùy chọn.
                - Không giải thích, chỉ trả về code python bắt đầu bằng 'from manim import *'.
                ";
            var code = await CallChatGptAsync(prompt);
            // Cắt bỏ các markdown, chỉ lấy phần code
            if (!string.IsNullOrEmpty(code))
            {
                code = code.Trim();
                if (code.StartsWith("```python")) code = code.Substring(9);
                if (code.StartsWith("```")) code = code.Substring(3);
                if (code.EndsWith("```")) code = code.Substring(0, code.Length - 3);
            }
            return code?.Trim();
        }

        public async Task<string> RunManimPythonAndGetImagePath(string manimCode)
        {
            // 1. Lưu code ra file tạm
            string sceneName = FindSceneNameFromManimCode(manimCode) ?? "AutoScene";
            string pyFile = $"auto_manim_{Guid.NewGuid().ToString("N").Substring(0, 8)}.py";
            await System.IO.File.WriteAllTextAsync(pyFile, manimCode, Encoding.UTF8);

            // 2. Gọi manim subprocess
            string arguments = $"-pql {pyFile} {sceneName} -s";
            var psi = new ProcessStartInfo
            {
                FileName = "manim",
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };
            var process = Process.Start(psi);
            string stdout = await process.StandardOutput.ReadToEndAsync();
            string stderr = await process.StandardError.ReadToEndAsync();
            process.WaitForExit();

            // 3. Tìm file ảnh đã sinh ra
            string outDir = $"media/images/{System.IO.Path.GetFileNameWithoutExtension(pyFile)}/480p15/";
            string imagePath = $"{outDir}{sceneName}_0000.png";
            if (System.IO.File.Exists(imagePath)) return imagePath;

            // Hoặc scan thư mục để lấy file PNG
            if (System.IO.Directory.Exists(outDir))
            {
                var files = System.IO.Directory.GetFiles(outDir, "*.png");
                if (files.Any()) return files.First();
            }
            return null;
        }

        // Helper: tìm tên scene trong code manim
        public string FindSceneNameFromManimCode(string code)
        {
            var match = System.Text.RegularExpressions.Regex.Match(code, @"class\s+(\w+)\s*\(\s*Scene\s*\)");
            if (match.Success) return match.Groups[1].Value;
            return null;
        }

        public async Task<List<ExtractedQuestionDto>> ExtractQuestionsFromTextAsync(string questionContent, string fileName, int grade)
        {
            var param = new TopicByGradeSpecParam { Grade = grade };
            var query = new GetTopicsByGradeQuery(param);
            var topicResponses = await _mediator.Send(query); 

            var topicNames = topicResponses.Select(x => x.Name).ToList();

            string prompt = $@"
            Đây là nội dung đề kiểm tra vật lý lấy từ file {fileName}:
            ---
            {questionContent}
            ---
            - Hãy tách riêng từng câu hỏi trong đề (bỏ số thứ tự).
            - Đối với mỗi câu hỏi, trả về dạng JSON:
              {{
                ""Content"": Nội dung câu hỏi,
                ""Type"": Loại câu hỏi (MultipleChoice/TrueFalse/ShortAnswer/Essay),
                ""Level"": ""Easy"" hoặc ""Medium"" hoặc ""Hard"" (chính xác, không dấu cách, không tiếng Việt, không viết hoa hết, không thừa khoảng trắng)
                ""TopicName"": Tên chủ đề (chọn từ danh sách sao cho phù hợp với câu hỏi từ list chủ đề :{topicNames}), chỉ chọn 1 và  không thay đổi nội dung chủ đề,
                ""Answer1"": ..., ""Answer2"": ..., // Các đáp án, thường là có 4 đáp án
                ""Grade"": Lớp, //Grade phải là số nguyên. Không để trong dấu nháy
                ""MediaBase64"": Base64 hình minh họa (nếu có), null nếu không có hình.
              }}
            - Trả về danh sách [ {{...}}, ... ].
            - Phần MultipleChoice trả ra các đáp án ko cần A., B., C., D. ở đầu
            - Phần TrueFalse/ShortAnswer/Essay lưu hết vào content, phần Answer để null (Phần TrueFalse 1 câu chỉ trả ra 1 lần)
            - Không trả lại bất cứ văn bản hay giải thích nào ngoài JSON!
            ";

            // --- Gọi OpenAI API trực tiếp ---
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
                throw new Exception("OpenAI API call failed: " + error);
            }

            var result = await response.Content.ReadFromJsonAsync<ChatGptResponse>();
            var resultText = result?.choices?.FirstOrDefault()?.message?.content;

            if (string.IsNullOrWhiteSpace(resultText))
                throw new Exception("Không nhận được dữ liệu từ ChatGPT!");

            // Có thể có markdown code block, cần lọc ra
            var jsonStart = resultText.IndexOf('[');
            var jsonEnd = resultText.LastIndexOf(']');
            if (jsonStart >= 0 && jsonEnd > jsonStart)
                resultText = resultText.Substring(jsonStart, jsonEnd - jsonStart + 1);

            var questions = JsonSerializer.Deserialize<List<ExtractedQuestionDto>>(resultText, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter() }
            });
            return questions;
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
