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
            - Tổng qua ma trận: {request.Description}
            Ma trận đề thi chi tiết:
            {request.Matrix}

            Yêu cầu:
            - Cái quan trọng nhất là phải sinh ra đúng số lượng câu hỏi dựa trên ma trận và tổng qua ma trận(kết hợp nhiều bài tập tính toán)
            - Câu hỏi bám sát kiến thức và mức độ theo ma trận.(những câu ở mức độ càng cao thì ra câu hỏi càng khó, những câu mức độ khó là những câu bài tập tính toán)
            - Chỉ trả về câu hỏi, không trả lời, không giải thích thêm.
            - Trả về kết quả dưới dạng JSON chuẩn với các trường: multiple_choice, true_false, short_answer, essay. Không trả về văn bản, không ghi chú gì ngoài JSON.
            - phần multiple_choice sinh ra question và các options (ở opstions không cần a,b,c,d);
            - Ở phần true_false:
                Mỗi câu hỏi là một object gồm hai trường:
                content: Nhận định hoặc câu hỏi tính toán chung cho cả câu hỏi.(ưu tiên kết hợp cả tính toán vào)
                options: Một mảng 4 đáp án cho câu hỏi(mỗi phát biểu có thể đúng/sai, lấy theo thứ tự từ ma trận), không cần chú thích đúng sai sau câu trả lời
                ví dụ có 8 câu truefalse là gồm 2 object 1 content và 4 options từ đó linh hoạt cho các trường hợp khác
            - Phần short_answer và essay thì chỉ có content thôi
            - Với 2-3 câu hỏi, thêm trường imagePrompt mô tả chi tiết hình ảnh minh họa cho câu hỏi (nếu phù hợp), phải phù hợp với môn vật lý.";

            var chatGptResponse = await _chatGptService.CallChatGptAsync(prompt);

            if (string.IsNullOrEmpty(chatGptResponse))
                return BadRequest("Lỗi khi gọi ChatGPT.");

            try
            {
                string cleanJson = chatGptResponse.Trim();
                if (cleanJson.StartsWith("```json"))
                {
                    cleanJson = cleanJson.Substring(7);
                }
                if (cleanJson.EndsWith("```"))
                {
                    cleanJson = cleanJson.Substring(0, cleanJson.Length - 3);
                }

                var questionsNode = JsonNode.Parse(cleanJson);

                // Multiple choice
                if (questionsNode?["multiple_choice"] is JsonArray mcArr)
                {
                    foreach (var item in mcArr)
                    {
                        if (item?["options"] is JsonArray optionsArr)
                        {
                            item["option1"] = optionsArr.Count > 0 ? optionsArr[0]?.ToString() : "";
                            item["option2"] = optionsArr.Count > 1 ? optionsArr[1]?.ToString() : "";
                            item["option3"] = optionsArr.Count > 2 ? optionsArr[2]?.ToString() : "";
                            item["option4"] = optionsArr.Count > 3 ? optionsArr[3]?.ToString() : "";
                            item.AsObject().Remove("options");
                        }
                    }
                }

                // True/False
                if (questionsNode?["true_false"] is JsonArray tfArr)
                {
                    foreach (var item in tfArr)
                    {
                        if (item?["statement"] != null)
                        {
                            item["content"] = item["statement"]?.ToString();
                            item.AsObject().Remove("statement");
                        }
                        else if (item?["question"] != null)
                        {
                            item["content"] = item["question"]?.ToString();
                            item.AsObject().Remove("question");
                        }

                        if (item?["options"] is JsonArray optionsArr)
                        {
                            for (int i = 0; i < optionsArr.Count; i++)
                            {
                                item[$"option{i + 1}"] = optionsArr[i]?.ToString();
                            }
                            item.AsObject().Remove("options");
                        }

                        if (item?["statements"] is JsonArray statementsArr)
                        {
                            for (int i = 0; i < statementsArr.Count; i++)
                            {
                                var statement = statementsArr[i];
                                if (statement?["statement"] != null)
                                    item[$"option{i + 1}"] = statement["statement"]?.ToString();
                                else
                                    item[$"option{i + 1}"] = statement?.ToString();
                            }
                            item.AsObject().Remove("statements");
                        }
                    }
                }

                // Short Answer
                if (questionsNode?["short_answer"] is JsonArray saArr)
                {
                    foreach (var item in saArr)
                    {
                        if (item?["question"] != null)
                        {
                            item["content"] = item["question"]?.ToString();
                            item.AsObject().Remove("question");
                        }
                        else if (item?["statement"] != null)
                        {
                            item["content"] = item["statement"]?.ToString();
                            item.AsObject().Remove("statement");
                        }
                    }
                }

                // Essay
                if (questionsNode?["essay"] is JsonArray essayArr)
                {
                    foreach (var item in essayArr)
                    {
                        if (item?["question"] != null)
                        {
                            item["content"] = item["question"]?.ToString();
                            item.AsObject().Remove("question");
                        }
                        else if (item?["statement"] != null)
                        {
                            item["content"] = item["statement"]?.ToString();
                            item.AsObject().Remove("statement");
                        }
                    }
                }

                var userIdStr = User.FindFirst("sub")?.Value
                ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst("userId")?.Value;

                if (string.IsNullOrWhiteSpace(userIdStr))
                    return Unauthorized("Không xác định được người dùng.");

                    using (var tx = await _context.Database.BeginTransactionAsync())
                    {
                        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userIdStr);
                        if (user == null) return Unauthorized("Không tìm thấy người dùng.");

                        if (user.Coin < 5)
                        {
                            await tx.RollbackAsync();
                            return BadRequest("Số dư xu không đủ (cần 5 xu).");
                        }

                        user.Coin -= 5;

                        var transaction = new PhyGen.Domain.Entities.Transaction
                        {
                            Id = Guid.NewGuid(),
                            UserId = user.Id,
                            CoinAfter = user.Coin - 5,
                            CoinBefore = user.Coin,
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
                return Ok(questionsNode);
            }
            catch (Exception ex)
            {
                // Nếu lỗi parse, trả về raw string để debug
                return Ok(new { raw = chatGptResponse, error = ex.Message });
            }
        }
    }
}
