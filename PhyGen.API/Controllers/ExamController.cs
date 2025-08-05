using DocumentFormat.OpenXml.Packaging;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhyGen.API.Mapping;
using PhyGen.API.Models;
using PhyGen.Application.Exams.Commands;
using PhyGen.Application.Exams.Queries;
using PhyGen.Application.Exams.Responses;
using PhyGen.Application.Mapping;
using PhyGen.Application.Users.Exceptions;
using PhyGen.Domain.Specs;
using PhyGen.Infrastructure.Service;
using PhyGen.Shared;
using PhyGen.Shared.Constants;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes; // Add this


namespace PhyGen.API.Controllers
{
    [Route("api/exams")]
    [ApiController]
    public class ExamController : BaseController<ExamController>
    {
        private readonly ChatGptService _chatGptService;
        private readonly IWebHostEnvironment _env;
        public ExamController(
             IMediator mediator,
             ILogger<ExamController> logger,
             ChatGptService chatGptService,
             IWebHostEnvironment env)
             : base(mediator, logger)
        {
            _chatGptService = chatGptService;
            _env = env;
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
        public async Task<IActionResult> GenerateExam([FromForm] GenerateExamRequest request)
        {
            if (request.MatrixFile == null || request.MatrixFile.Length == 0)
                return BadRequest("File ma trận đề không được để trống.");

            string matrixContent = "";
            using (var ms = new MemoryStream())
            {
                await request.MatrixFile.CopyToAsync(ms);
                ms.Position = 0;
                using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(ms, false))
                {
                    matrixContent = ConvertWordToText(wordDoc);
                }
            }

            string prompt = $@"
            Bạn là giáo viên vật lý. Đọc ma trận hãy tạo nội dung đề thi dựa theo ma trận bên dưới, gồm đầy đủ các phần có trong ma trận đề thi:

            Thông tin đề thi:
            - Tiêu đề: {request.Title}
            - Loại kỳ thi: {request.ExamType}
            - Khối lớp: {request.Grade}
            - Năm học: {request.Year}

            Ma trận đề thi chi tiết:
            {matrixContent}

            Yêu cầu:
            - Cái quan trọng nhất là phải sinh ra đúng số lượng câu hỏi dựa trên ma trận (đan xen giữa lý thuyết và bài tập tính toán)
            - Câu hỏi bám sát kiến thức và mức độ theo ma trận.(những câu ở mức độ càng cao thì ra câu hỏi càng khó, những câu mức độ khó là những câu bài tập tính toán)
            - Chỉ trả về câu hỏi, không trả lời, không giải thích thêm.
            - Trả về kết quả dưới dạng JSON chuẩn với các trường: multiple_choice, true_false, short_answer, essay. Không trả về văn bản, không ghi chú gì ngoài JSON.
            - phần multiple_choice sinh ra question và các options (ở opstions không cần a,b,c,d);
            - Ở phần true_false:
                + Mỗi câu là một nhóm gồm nhiều ý nhỏ (mỗi ý là một nhận định hoặc kết quả), mỗi ý này là một câu trắc nghiệm đúng/sai riêng biệt.
                + Trả về mỗi câu dưới dạng một object có trường `content` (mô tả nội dung câu hỏi chung), trường `options` (mảng các nhận định, mỗi nhận định là một câu hỏi nhỏ đúng/sai).            
            - Phần short_answer và essay thì chỉ có content thôi";
            
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

                // Multiple choice (giữ nguyên như cũ)
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
                        // Đổi statement/question => content
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

                        // options
                        if (item?["options"] is JsonArray optionsArr)
                        {
                            for (int i = 0; i < optionsArr.Count; i++)
                            {
                                item[$"option{i + 1}"] = optionsArr[i]?.ToString();
                            }
                            item.AsObject().Remove("options");
                        }

                        // statements
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

                return Ok(questionsNode);
            }
            catch (Exception ex)
            {
                // Nếu lỗi parse, trả về raw string để debug
                return Ok(new { raw = chatGptResponse, error = ex.Message });
            }

        }
        // Hàm chuyển file Word sang text
        private string ConvertWordToText(WordprocessingDocument doc)
        {
            var sb = new StringBuilder();
            var body = doc.MainDocumentPart.Document.Body;
            foreach (var para in body.Elements<DocumentFormat.OpenXml.Wordprocessing.Paragraph>())
            {
                sb.AppendLine(para.InnerText);
            }
            return sb.ToString();
        }
    }
}
