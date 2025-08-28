using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhyGen.API.Mapping;
using PhyGen.API.Models;
using PhyGen.Application.Exams.Commands;
using PhyGen.Application.Mapping;
using PhyGen.Application.Notification.Commands;
using PhyGen.Application.Questions.Commands;
using PhyGen.Application.Questions.Dtos;
using PhyGen.Application.Questions.Interfaces;
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
using System.Text.RegularExpressions;
using static ImageConvertHelper;

namespace PhyGen.API.Controllers
{
    [Route("api/questions")]
    [ApiController]
    public class QuestionController : BaseController<QuestionController>
    {
        private readonly ChatGptService _chatGptService;
        private readonly IMediator _mediator;
        private readonly IImageStorage _imageStorage;

        public QuestionController(IImageStorage imageStorage,
            IMediator mediator,
            ILogger<QuestionController> logger,
            ChatGptService chatGptService
        ) : base(mediator, logger)
        {
            _mediator = mediator;
            _imageStorage = imageStorage;
            _chatGptService = chatGptService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<QuestionResponse>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllQuestions([FromQuery] QuestionSpecParam questionSpecParam)
        {
            var user = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(user))
                return Unauthorized(new UserNotFoundException());
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
            var request = new GetQuestionsByGradeQuery(questionGradeParam);
            return await ExecuteAsync<GetQuestionsByGradeQuery, Pagination<QuestionResponse>>(request);
        }

        [HttpGet("for-create-exam")]
        [ProducesResponseType(typeof(ApiResponse<Pagination<QuestionResponse>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetQuestionsForCreateExam([FromQuery] QuestionSpecParam questionSpecParam)
        {
            var user = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(user))
                return Unauthorized(new UserNotFoundException());

            questionSpecParam.IsDuplicate = false;

            var query = new GetQuestionsQuery(questionSpecParam);
            return await ExecuteAsync<GetQuestionsQuery, Pagination<QuestionResponse>>(query);
        }


        [Authorize]
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
        //[Authorize]
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

        [HttpPatch("status")]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateQuestionStatusRequest request)
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

            var command = AppMapper<ModelMappingProfile>.Mapper.Map<UpdateQuestionStatusCommand>(request);
            return await ExecuteAsync<UpdateQuestionStatusCommand, Unit>(command);
        }

        [HttpDelete]
        //[Authorize]
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
        public async Task<ActionResult<List<ExtractedQuestionDto>>> ExtractQuestionsFromFile([FromForm] FileUploadRequest request)
        {
            if (request.File == null || request.File.Length == 0)
                return BadRequest("File rỗng.");

            // 1) Đọc Word -> gom text + ảnh
            List<QuestionWithImages> questionsWithImages;
            using (var ms = new MemoryStream())
            {
                await request.File.CopyToAsync(ms);
                ms.Position = 0;
                using var wordDoc = WordprocessingDocument.Open(ms, false);
                questionsWithImages = ExtractQuestionsWithImages(wordDoc);
            }

            // 2) Gọi AI tách câu hỏi
            string fullText = string.Join("\n\n", questionsWithImages.Select(q => q.Content));
            var questions = await _chatGptService.ExtractQuestionsFromTextAsync(
                fullText,
                request.File.FileName,
                request.Grade
            );

            // Chuẩn bị danh sách nguồn (từ Word) & đích (từ GPT) để so khớp
            var srcList = questionsWithImages
                .Select((qw, idx) => new
                {
                    Index = idx,
                    Head = ExtractHead(qw.Content ?? string.Empty),
                    Canon = Canonicalize(qw.Content ?? string.Empty),
                    Item = qw
                })
                .ToList();

            var tgtList = questions
                .Select((q, idx) => new
                {
                    Index = idx,
                    Head = ExtractHead(q.Content ?? string.Empty),
                    Canon = Canonicalize(q.Content ?? string.Empty),
                    Item = q
                })
                .ToList();

            // 3) Map ảnh -> URL Cloudinary THEO NỘI DUNG
            var uploadedByHash = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            var usedSrc = new HashSet<int>(); // nếu muốn 1 ảnh dùng cho nhiều câu, bỏ dòng check ở dưới

            foreach (var tgt in tgtList)
            {
                double bestScore = 0.0;
                int bestSrcIdx = -1;

                foreach (var src in srcList)
                {
                    if (usedSrc.Contains(src.Index)) continue; 

                    var score = MatchScore(src.Head, tgt.Head);
                    if (Regex.IsMatch(src.Item.Content ?? "", @"hình\s+(bên|vẽ)", RegexOptions.IgnoreCase))
                        score += 0.05;

                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestSrcIdx = src.Index;
                    }
                }

                // ngưỡng nhận: có thể hạ xuống 0.2 nếu đề có phrasing ngắn
                if (bestSrcIdx < 0 || bestScore < 0.25) continue;

                var matchedSrc = srcList.First(s => s.Index == bestSrcIdx);
                usedSrc.Add(bestSrcIdx);

                tgt.Item.MediaUrls ??= new List<string>();
                var urlsInThisQuestion = new HashSet<string>(tgt.Item.MediaUrls);

                foreach (var img in matchedSrc.Item.Images)
                {
                    var normalizedBytes = ImageConvertHelper.ToCloudinaryFriendlyPngIfNeeded(
                        img.Data, img.ContentType, out var normalizedCt, out var ext);

                    var key = Sha256Hex(normalizedBytes);

                    if (!uploadedByHash.TryGetValue(key, out var url))
                    {
                        var hintBase = matchedSrc.Head.Length >= 8 ? matchedSrc.Head.Substring(0, 8) : "q";
                        hintBase = Regex.Replace(hintBase, @"\s+", "_");
                        url = await _imageStorage.SaveAsync(normalizedBytes, normalizedCt, hintBase);
                        uploadedByHash[key] = url;
                    }

                    if (urlsInThisQuestion.Add(url))
                        tgt.Item.MediaUrls.Add(url);
                }
            }
            return Ok(questions);
        }

        [HttpPost("check-file")]
        public async Task<IActionResult> CheckFileQuestion([FromForm] FileUploadRequest request)
        {
            if (request.File == null || request.File.Length == 0)
                return BadRequest("File rỗng.");

            string fullText;
            using (var ms = new MemoryStream())
            {
                await request.File.CopyToAsync(ms);
                ms.Position = 0;

                using (var wordDoc = WordprocessingDocument.Open(ms, false))
                {
                    fullText = DocxReader.ReadFullText(wordDoc);
                }
            }

            // Gọi ChatGPT để kiểm tra có phải đề Vật lý không
            var isPhysicsExam = await _chatGptService.IsPhysicsExamAsync(
                request.File.FileName,
                fullText,
                HttpContext.RequestAborted
            );

            return Ok(new { isPhysicsExam });
        }
    }
}