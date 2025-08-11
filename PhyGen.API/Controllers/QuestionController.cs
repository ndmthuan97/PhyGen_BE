using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhyGen.API.Mapping;
using PhyGen.API.Models;
using PhyGen.Application.Mapping;
using PhyGen.Application.Notification.Commands;
using PhyGen.Application.Questions.Commands;
using PhyGen.Application.Questions.Dtos;
using PhyGen.Application.Questions.Queries;
using PhyGen.Application.Questions.Request;
using PhyGen.Application.Questions.Responses;
using PhyGen.Application.Users.Exceptions;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Specs;
using PhyGen.Domain.Specs.Question;
using PhyGen.Infrastructure.Persistence.DbContexts;
using PhyGen.Infrastructure.Service;
using PhyGen.Shared;
using PhyGen.Shared.Constants;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace PhyGen.API.Controllers
{
    [Route("api/questions")]
    [ApiController]
    public class QuestionController : BaseController<QuestionController>
    {
        private readonly CloudinaryService _cloudinaryService;
        private readonly AppDbContext _dbContext;
        private readonly ChatGptService _chatGptService;
        private readonly TopicService _topicService;
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public QuestionController(
            IMediator mediator,
            ILogger<QuestionController> logger,
            TopicService topicService,
            CloudinaryService cloudinaryService,
            AppDbContext dbContext,
            ChatGptService chatGptService, IHttpContextAccessor httpContextAccessor
        ) : base(mediator, logger)
        {
            _topicService = topicService;
            _cloudinaryService = cloudinaryService;
            _dbContext = dbContext;
            _chatGptService = chatGptService;
            _mediator = mediator;
            _httpContextAccessor = httpContextAccessor;

        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<QuestionResponse>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllQuestions([FromQuery] QuestionSpecParam questionSpecParam)
        {
            var user = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(user))
                return Unauthorized(new UserNotFoundException());

            var isAdmin = User.IsInRole(nameof(Role.Admin));

            if (!isAdmin)
            {
                questionSpecParam.CreatedByList = new List<string> { user, "Admin" };
            }
            else
            {
                questionSpecParam.CreatedByList = new List<string> { "Admin" };
            }

            var query = new GetQuestionsQuery(questionSpecParam);
            return await ExecuteAsync<GetQuestionsQuery, Pagination<QuestionResponse>>(query);
        }

        [HttpGet("{questionId}")]
        [ProducesResponseType(typeof(ApiResponse<QuestionResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetQuestionById(Guid questionId)
        {
            var request = new GetQuestionByIdQuery(questionId);
            return await ExecuteAsync<GetQuestionByIdQuery, QuestionResponse>(request);
        }

        [HttpGet("topic")]
        [ProducesResponseType(typeof(ApiResponse<Pagination<QuestionResponse>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetQuestionsByTopicId([FromQuery] QuestionByTopicSpecParam param)
        {
            var user = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(user))
                return Unauthorized(new UserNotFoundException());

            var isAdmin = User.IsInRole(nameof(Role.Admin));

            if (!isAdmin)
            {
                param.CreatedByList = new List<string> { user, "Admin" };
            }
            else
            {
                param.CreatedByList = new List<string> { "Admin" };
            }

            var request = new GetQuestionsByTopicIdQuery(param);
            return await ExecuteAsync<GetQuestionsByTopicIdQuery, Pagination<QuestionResponse>>(request);
        }

        [HttpGet("level&type")]
        [ProducesResponseType(typeof(ApiResponse<Pagination<QuestionResponse>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetQuestionsByLevelAndType([FromQuery] QuestionSpecParam questionSpecParam)
        {
            var user = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(user))
                return Unauthorized(new UserNotFoundException());

            var isAdmin = User.IsInRole(nameof(Role.Admin));

            if (!isAdmin)
            {
                questionSpecParam.CreatedByList = new List<string> { user, "Admin" };
            }
            else
            {
                questionSpecParam.CreatedByList = new List<string> { "Admin" };
            }

            var request = new GetQuestionsByLevelAndTypeQuery(questionSpecParam);
            return await ExecuteAsync<GetQuestionsByLevelAndTypeQuery, Pagination<QuestionResponse>>(request);
        }

        [HttpGet("grade")]
        [ProducesResponseType(typeof(ApiResponse<Pagination<QuestionResponse>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetQuestionsByGrade([FromQuery] QuestionByGradeSpecParam questionGradeParam)
        {
            var user = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(user))
                return Unauthorized(new UserNotFoundException());

            var isAdmin = User.IsInRole(nameof(Role.Admin));

            if (!isAdmin)
            {
                questionGradeParam.CreatedByList = new List<string> { user, "Admin" };
            }
            else
            {
                questionGradeParam.CreatedByList = new List<string> { "Admin" };
            }

            var request = new GetQuestionsByGradeQuery(questionGradeParam);
            return await ExecuteAsync<GetQuestionsByGradeQuery, Pagination<QuestionResponse>>(request);
        }

        //[Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<QuestionResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateQuestion([FromBody] CreateQuestionRequest request)
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

            var command = AppMapper<ModelMappingProfile>.Mapper.Map<CreateQuestionCommand>(request);

            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(userEmail))
                return Unauthorized(new UserNotFoundException());

            var isAdmin = User.IsInRole(nameof(Role.Admin));

            if (!isAdmin)
            {
                command.CreatedBy = userEmail;
            }
            else
            {
                command.CreatedBy = "Admin";
            }

            return await ExecuteAsync<CreateQuestionCommand, QuestionResponse>(command);
        }

        [HttpPut]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<Unit>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateQuestion([FromBody] UpdateQuestionRequest request)
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

            var command = AppMapper<ModelMappingProfile>.Mapper.Map<UpdateQuestionCommand>(request);
            return await ExecuteAsync<UpdateQuestionCommand, Unit>(command);
        }

        [HttpDelete]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<Unit>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteQuestion([FromBody] DeleteQuestionRequest request)
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

            var command = AppMapper<ModelMappingProfile>.Mapper.Map<DeleteQuestionCommand>(request);
            return await ExecuteAsync<DeleteQuestionCommand, Unit>(command);
        }

        [HttpPost("extract-from-file")]
        [Authorize]
        public async Task<IActionResult> ExtractQuestionsFromFile([FromForm] FileUploadRequest request)
        {
            if (request.File == null || request.File.Length == 0)
                return BadRequest("File rỗng.");

            List<QuestionWithImages> questionsWithImages;
            using (var ms = new MemoryStream())
            {
                await request.File.CopyToAsync(ms);
                ms.Position = 0;
                using (var wordDoc = WordprocessingDocument.Open(ms, false))
                {
                    questionsWithImages = ExtractQuestionsWithImages(wordDoc);
                }
            }

            // Gọi AI extract các câu hỏi (dựa vào text) -> có thể truyền toàn bộ text hoặc từng câu một tùy cách bạn muốn (ở đây truyền toàn bộ)
            string fullText = string.Join("\n\n", questionsWithImages.Select(q => q.Content));
            List<ExtractedQuestionDto> questions = await _chatGptService.ExtractQuestionsFromTextAsync(fullText, request.File.FileName, request.Grade);

            int savedCount = 0;
            for (int i = 0; i < questions.Count; i++)
            {
                var q = questions[i];
                var topicId = await _topicService.GetTopicIdByNameAsync(q.TopicName);
                if (topicId == null)
                    return BadRequest($"Không tìm thấy chủ đề: {q.TopicName}");

                var question = new Question
                {
                    TopicId = (Guid)topicId,
                    Content = q.Content,
                    Type = q.Type,
                    Level = q.Level,
                    Answer1 = q.Answer1,
                    Answer2 = q.Answer2,
                    Answer3 = q.Answer3,
                    Answer4 = q.Answer4,
                    Answer5 = q.Answer5,
                    Answer6 = q.Answer6,
                    Grade = q.Grade,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "User"
                };
                _dbContext.Questions.Add(question);
                await _dbContext.SaveChangesAsync();

                if (i < questionsWithImages.Count)
                {
                    foreach (var imgBytes in questionsWithImages[i].Images)
                    {
                        string url = await _cloudinaryService.UploadImageAsync(new MemoryStream(imgBytes), $"{Guid.NewGuid()}.png");
                        if (url == null)
                        {
                            Console.WriteLine("Lỗi upload ảnh lên Cloudinary! Không lưu QuestionMedia.");
                            continue;
                        }
                        Console.WriteLine("Ảnh upload thành công: " + url);

                        var questionMedia = new QuestionMedia
                        {
                            QuestionId = question.Id,
                            Url = url
                        };
                        _dbContext.QuestionMedias.Add(questionMedia);
                        await _dbContext.SaveChangesAsync();
                    }

                }

                // Nếu vẫn muốn check MediaBase64 từ AI (nâng cao):
                if (!string.IsNullOrWhiteSpace(q.MediaBase64))
                {
                    var imageBytes = Convert.FromBase64String(q.MediaBase64);
                    using (var imgStream = new MemoryStream(imageBytes))
                    {
                        string imageUrl = await _cloudinaryService.UploadImageAsync(imgStream, $"{Guid.NewGuid()}.png");
                        var questionMedia = new QuestionMedia
                        {
                            QuestionId = question.Id,
                            Url = imageUrl
                        };
                        _dbContext.QuestionMedias.Add(questionMedia);
                        await _dbContext.SaveChangesAsync();
                    }
                }
                savedCount++;
            }

            var result = new { Message = $"Đã lưu {savedCount} câu hỏi." };

            // Tạo notification
            await _mediator.Send(new CreateNotificationCommand
            {
                UserId = Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var guid)
                ? guid
                : throw new UnauthorizedAccessException("UserId trong token không hợp lệ."),
                Title = "Tạo câu hỏi thành công",
                Message = $"Bạn đã lưu thành công {savedCount} câu hỏi.",
                CreatedAt = DateTime.UtcNow
            });

            return Ok(result);
        }

        private class QuestionWithImages
        {
            public string Content { get; set; }
            public List<byte[]> Images { get; set; } = new List<byte[]>();
        }

        // Duyệt Word, mapping từng câu hỏi với list ảnh
        private List<QuestionWithImages> ExtractQuestionsWithImages(WordprocessingDocument doc)
        {
            var result = new List<QuestionWithImages>();
            QuestionWithImages current = null;

            void ProcessParagraph(DocumentFormat.OpenXml.Wordprocessing.Paragraph para)
            {
                var text = para.InnerText?.Trim();
                if (!string.IsNullOrWhiteSpace(text))
                {
                    current = new QuestionWithImages { Content = text };
                    result.Add(current);
                }

                foreach (var blip in para.Descendants<DocumentFormat.OpenXml.Drawing.Blip>())
                {
                    var imgPart = (ImagePart)doc.MainDocumentPart.GetPartById(blip.Embed.Value);
                    using (var imgStream = imgPart.GetStream())
                    using (var ms = new MemoryStream())
                    {
                        imgStream.CopyTo(ms);
                        if (current == null)
                        {
                            current = new QuestionWithImages();
                            result.Add(current);
                        }
                        current.Images.Add(ms.ToArray());
                    }
                }
            }

            foreach (var elem in doc.MainDocumentPart.Document.Body.Elements())
            {
                if (elem is DocumentFormat.OpenXml.Wordprocessing.Paragraph para)
                {
                    ProcessParagraph(para);
                }
                else if (elem is DocumentFormat.OpenXml.Wordprocessing.Table table)
                {
                    foreach (var row in table.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>())
                    {
                        foreach (var cell in row.Elements<DocumentFormat.OpenXml.Wordprocessing.TableCell>())
                        {
                            foreach (var innerPara in cell.Elements<DocumentFormat.OpenXml.Wordprocessing.Paragraph>())
                            {
                                ProcessParagraph(innerPara);
                            }
                        }
                    }
                }
            }
            return result;
        }

        [HttpPost("check-file")]
        public async Task<IActionResult> CheckFileQuestion([FromForm] FileUploadRequest request)
        {
            if (request.File == null || request.File.Length == 0)
                return BadRequest("File rỗng.");

            List<QuestionWithImages> questionsWithImages;
            using (var ms = new MemoryStream())
            {
                await request.File.CopyToAsync(ms);
                ms.Position = 0;
                using (var wordDoc = WordprocessingDocument.Open(ms, false))
                {
                    questionsWithImages = ExtractQuestionsWithImages(wordDoc);
                }
            }

            var fullText = string.Join("\n\n", questionsWithImages
                .Select(q => q.Content)
                .Where(s => !string.IsNullOrWhiteSpace(s)));

            // Không cần dùng request.Grade ở đây
            var isPhysicsExam = await _chatGptService.IsPhysicsExamAsync(
                request.File.FileName,
                fullText,
                HttpContext.RequestAborted
            );

            return Ok(new { isPhysicsExam });
        }
    }
}