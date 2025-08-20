using DocumentFormat.OpenXml.Packaging;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhyGen.API.Mapping;
using PhyGen.API.Models;
using PhyGen.Application.Exams.Commands;
using PhyGen.Application.Exams.Queries;
using PhyGen.Application.Exams.Responses;
using PhyGen.Application.Mapping;
using PhyGen.Application.Notification.Commands;
using PhyGen.Application.Users.Exceptions;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Specs;
using PhyGen.Infrastructure.Persistence.DbContexts;
using PhyGen.Infrastructure.Service;
using PhyGen.Shared;
using PhyGen.Shared.Constants;
using System.Diagnostics;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace PhyGen.API.Controllers
{
    [Route("api/exams")]
    [ApiController]
    public class ExamController : BaseController<ExamController>
    {
        private readonly ChatGptService _chatGptService;
        private readonly IWebHostEnvironment _env;
        private readonly AppDbContext _context;

        public ExamController(
             IMediator mediator,
             ILogger<ExamController> logger,
             ChatGptService chatGptService,
             IWebHostEnvironment env,
             AppDbContext context)
             : base(mediator, logger)
        {
            _chatGptService = chatGptService;
            _env = env;
            _context = context;
        }

        [HttpGet("{examId}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<ExamResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetExamById(Guid examId)
        {
            var user = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(user))
                return Unauthorized(new UserNotFoundException());

            var query = new GetExamByIdQuery(examId);
            return await ExecuteAsync<GetExamByIdQuery, ExamResponse>(query);
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<Pagination<ExamResponse>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllExams([FromQuery] ExamSpecParam param)
        {
            var query = new GetExamsQuery(param);
            return await ExecuteAsync<GetExamsQuery, Pagination<ExamResponse>>(query);
        }

        [HttpGet("{examId}/detail")]
        [ProducesResponseType(typeof(ApiResponse<ExamDetailResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetExamDetail(Guid examId)
        {
            var query = new GetExamDetailQuery(examId);
            return await ExecuteAsync<GetExamDetailQuery, ExamDetailResponse>(query);
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<ExamResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateExam([FromBody] CreateExamRequest request)
        {
            if (request == null)
            {
                return BadRequest(new ApiResponse<object>
                {
                    StatusCode = (int)Shared.Constants.StatusCode.ModelInvalid,
                    Message = ResponseMessages.GetMessage(Shared.Constants.StatusCode.ModelInvalid),
                    Errors = ["The request body does not contain required fields"]
                });
            }

            var user = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(user))
                return Unauthorized(new UserNotFoundException());

            var command = AppMapper<ModelMappingProfile>.Mapper.Map<CreateExamCommand>(request);
            return await ExecuteAsync<CreateExamCommand, ExamResponse>(command);
        }

        [HttpPut]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<Unit>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateExam([FromBody] UpdateExamRequest request)
        {
            if (request == null)
            {
                return BadRequest(new ApiResponse<object>
                {
                    StatusCode = (int)Shared.Constants.StatusCode.ModelInvalid,
                    Message = ResponseMessages.GetMessage(Shared.Constants.StatusCode.ModelInvalid),
                    Errors = ["The request body does not contain required fields"]
                });
            }

            var user = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(user))
                return Unauthorized(new UserNotFoundException());

            var command = AppMapper<ModelMappingProfile>.Mapper.Map<UpdateExamCommand>(request);
            return await ExecuteAsync<UpdateExamCommand, Unit>(command);
        }

        [HttpPatch("status")]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateExamStatusRequest request)
        {
            if (request == null || request.Ids == null || !request.Ids.Any())
            {
                return BadRequest(new ApiResponse<object>
                {
                    StatusCode = (int)Shared.Constants.StatusCode.ModelInvalid,
                    Message = ResponseMessages.GetMessage(Shared.Constants.StatusCode.ModelInvalid),
                    Errors = ["The request body does not contain required fields"]
                });
            }

            var command = AppMapper<ModelMappingProfile>.Mapper.Map<UpdateExamStatusCommand>(request);
            return await ExecuteAsync<UpdateExamStatusCommand, Unit>(command);
        }

        [HttpDelete]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<Unit>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteExam([FromBody] DeleteExamRequest request)
        {
            if (request == null)
            {
                return BadRequest(new ApiResponse<object>
                {
                    StatusCode = (int)Shared.Constants.StatusCode.ModelInvalid,
                    Message = ResponseMessages.GetMessage(Shared.Constants.StatusCode.ModelInvalid),
                    Errors = ["The request body does not contain required fields"]
                });
            }

            var user = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(user))
                return Unauthorized(new UserNotFoundException());

            var command = AppMapper<ModelMappingProfile>.Mapper.Map<DeleteExamCommand>(request);
            return await ExecuteAsync<DeleteExamCommand, Unit>(command);
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateExam([FromBody] GenerateExamRequest request)
        {
            string prompt = $@"
            Bạn là giáo viên vật lý. Đọc ma trận hãy tạo nội dung đề thi dựa theo ma trận bên dưới, gồm đầy đủ các phần có trong ma trận đề thi:

            Thông tin đề thi:
            - Loại kỳ thi: {request.ExamType}
            - Khối lớp: {request.Grade}
            - Năm học: {request.Year}
            - Tổng quan ma trận: {request.Description}
            Ma trận đề thi chi tiết:
            {request.Matrix}

            Yêu cầu:
            - Cái quan trọng nhất là phải sinh ra đúng số lượng câu hỏi dựa trên ma trận và tổng qua ma trận(kết hợp nhiều bài tập tính toán)
            - Câu hỏi bám sát kiến thức và mức độ theo ma trận.(những câu ở mức độ càng cao thì ra câu hỏi càng khó, những câu mức độ khó là những câu bài tập tính toán)
            - Chỉ trả về câu hỏi, không trả lời, không giải thích thêm.
            - Trả về kết quả dưới dạng JSON chuẩn với các trường: multiple_choice, true_false, short_answer, essay. Không trả về văn bản, không ghi chú gì ngoài JSON.
            - phần multiple_choice sinh ra question và các options (ở opstions không cần a,b,c,d);
            - Ở phần true_false: 
                - Nếu là câu hỏi thì có trường question và options (options là mảng gồm 2 phần tử: đúng và sai)
                - Nếu là câu statement thì có trường statement và options (options là mảng gồm 2 phần tử: đúng và sai)
            - Phần short_answer và essay thì chỉ có content thôi,
            - QUAN TRỌNG: trả về JSON để ko bị lỗi khi parse bằng System.Text.Json ngôn ngữ C# ASP.NET";

            var chatGptResponse = await _chatGptService.CallChatGptAsync(prompt);

            if (string.IsNullOrEmpty(chatGptResponse))
                return BadRequest("Lỗi khi gọi ChatGPT.");

            try
            {
                // 3) Làm sạch code fence TỐI ƯU (index-based, hạn chế copy chuỗi)
                string cleanJson = CleanJsonFences(chatGptResponse);

                // 4) Parse JSON (giữ JsonNode như cũ để không đổi kết cấu)
                var questionsNode = JsonNode.Parse(cleanJson);

                // 5) Chuẩn hoá schema (tối ưu duyệt & gán)
                NormalizeQuestionsNode(questionsNode);

                // 6) Xác định userId
                var userIdStr =
                    User.FindFirst("sub")?.Value
                    ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                    ?? User.FindFirst("userId")?.Value;

                if (string.IsNullOrWhiteSpace(userIdStr))
                    return Unauthorized("Không xác định được người dùng.");

                // 7) Chỉ mở transaction khi thật sự cần trừ xu (User role)
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userIdStr);
                if (user == null) return Unauthorized("Không tìm thấy người dùng.");

                if (string.Equals(user.Role, "User", StringComparison.OrdinalIgnoreCase))
                {
                    if (user.Coin < 5)
                        return BadRequest("Số dư xu không đủ (cần 5 xu).");

                    using (var tx = await _context.Database.BeginTransactionAsync())
                    {
                        var before = user.Coin;
                        user.Coin = before - 5;

                        var transaction = new PhyGen.Domain.Entities.Transaction
                        {
                            Id = Guid.NewGuid(),
                            UserId = user.Id,
                            CoinAfter = user.Coin,
                            CoinBefore = before,
                            CoinChange = -5,
                            TypeChange = "Generate",
                            PaymentlinkID = null,
                            CreatedAt = DateTime.UtcNow
                        };

                        _context.Transactions.Add(transaction);
                        await _context.SaveChangesAsync();

                        await _mediator.Send(new CreateNotificationCommand
                        {
                            UserId = user.Id,
                            Title = "Giao dịch thành công",
                            Message = $"Bạn đã thanh toán thành công 5 xu để tạo đề thi. Số dư còn: {user.Coin} xu.",
                            CreatedAt = DateTime.UtcNow
                        });

                        await tx.CommitAsync();
                    }
                }

                // 8) Trả kết quả (giữ nguyên kiểu trả)
                return Ok(questionsNode);
            }
            catch (Exception ex)
            {
                // Nếu lỗi parse, trả về raw string để debug (giữ nguyên)
                return Ok(new { raw = chatGptResponse, error = ex.Message });
            }
        }
        /// <summary>
        /// Làm sạch code fence và cắt đúng vùng JSON một lần (ít copy chuỗi).
        /// Hỗ trợ cả block ```json ... ``` hoặc dư văn bản ngoài.
        /// </summary>
        private static string CleanJsonFences(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return string.Empty;

            ReadOnlySpan<char> span = raw.AsSpan().Trim();

            // Cắt ```json ... ``` ở đầu nếu có
            const string fenceJson = "```json";
            if (span.StartsWith(fenceJson.AsSpan(), StringComparison.OrdinalIgnoreCase))
            {
                span = span.Slice(fenceJson.Length).TrimStart(); // bỏ tiền tố ```json
            }

            // Cắt ``` ở cuối nếu có
            const string fence = "```";
            if (span.EndsWith(fence.AsSpan(), StringComparison.Ordinal))
            {
                span = span.Slice(0, span.Length - fence.Length).TrimEnd();
            }

            // Nếu vẫn còn văn bản ngoài JSON -> ưu tiên object { ... } nếu có
            int objStart = span.IndexOf('{');
            int objEnd = span.LastIndexOf('}');
            if (objStart >= 0 && objEnd > objStart)
            {
                return span.Slice(objStart, objEnd - objStart + 1).ToString();
            }

            // Hoặc mảng [ ... ]
            int arrStart = span.IndexOf('[');
            int arrEnd = span.LastIndexOf(']');
            if (arrStart >= 0 && arrEnd > arrStart)
            {
                return span.Slice(arrStart, arrEnd - arrStart + 1).ToString();
            }

            // Không tìm thấy -> trả về phần đã trim (giữ nguyên hành vi)
            return span.ToString();
        }

        /// <summary>
        /// Chuẩn hoá schema JSON in-place (giữ logic cũ, tối ưu truy cập JsonNode)
        /// </summary>
        private static void NormalizeQuestionsNode(JsonNode? root)
        {
            if (root is null) return;

            // multiple_choice: options[] -> option1..4
            if (root["multiple_choice"] is JsonArray mcArr)
            {
                foreach (var node in mcArr)
                {
                    if (node is not JsonObject obj) continue;

                    if (obj["options"] is JsonArray options)
                    {
                        for (int i = 0; i < options.Count; i++)
                        {
                            if (options[i] is JsonValue v && v.TryGetValue<string>(out var s))
                                obj[$"option{i + 1}"] = s;
                            else
                                obj[$"option{i + 1}"] = options[i]?.ToJsonString() ?? "";
                        }
                        obj.Remove("options");
                    }
                }
            }

            // true_false: statement|question -> content; options[]/statements[] -> option1..n
            if (root["true_false"] is JsonArray tfArr)
            {
                foreach (var node in tfArr)
                {
                    if (node is not JsonObject obj) continue;

                    if (obj.ContainsKey("statement"))
                    {
                        obj["content"] = obj["statement"]?.GetValue<string>();
                        obj.Remove("statement");
                    }
                    else if (obj.ContainsKey("question"))
                    {
                        obj["content"] = obj["question"]?.GetValue<string>();
                        obj.Remove("question");
                    }

                    if (obj["options"] is JsonArray options)
                    {
                        for (int i = 0; i < options.Count; i++)
                        {
                            if (options[i] is JsonValue v && v.TryGetValue<string>(out var s))
                                obj[$"option{i + 1}"] = s;
                            else
                                obj[$"option{i + 1}"] = options[i]?.ToJsonString();
                        }
                        obj.Remove("options");
                    }

                    if (obj["statements"] is JsonArray statements)
                    {
                        for (int i = 0; i < statements.Count; i++)
                        {
                            string? text = null;
                            if (statements[i] is JsonObject so && so["statement"] is JsonNode stVal)
                                text = stVal.GetValue<string>();
                            else if (statements[i] is JsonValue sv && sv.TryGetValue<string>(out var s2))
                                text = s2;
                            else
                                text = statements[i]?.ToJsonString();

                            obj[$"option{i + 1}"] = text;
                        }
                        obj.Remove("statements");
                    }
                }
            }

            // short_answer: question|statement -> content
            if (root["short_answer"] is JsonArray saArr)
            {
                foreach (var node in saArr)
                {
                    if (node is not JsonObject obj) continue;
                    if (obj.ContainsKey("question"))
                    {
                        obj["content"] = obj["question"]?.GetValue<string>();
                        obj.Remove("question");
                    }
                    else if (obj.ContainsKey("statement"))
                    {
                        obj["content"] = obj["statement"]?.GetValue<string>();
                        obj.Remove("statement");
                    }
                }
            }

            // essay: question|statement -> content
            if (root["essay"] is JsonArray essayArr)
            {
                foreach (var node in essayArr)
                {
                    if (node is not JsonObject obj) continue;
                    if (obj.ContainsKey("question"))
                    {
                        obj["content"] = obj["question"]?.GetValue<string>();
                        obj.Remove("question");
                    }
                    else if (obj.ContainsKey("statement"))
                    {
                        obj["content"] = obj["statement"]?.GetValue<string>();
                        obj.Remove("statement");
                    }
                }
            }
        } 
    }
}
