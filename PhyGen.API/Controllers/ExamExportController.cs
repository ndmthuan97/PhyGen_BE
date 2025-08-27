using DocumentFormat.OpenXml.Packaging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhyGen.Application.Exams.Interfaces;
using PhyGen.Application.Exams.Models;
using PhyGen.Domain.Entities;
using PhyGen.Infrastructure.Persistence.DbContexts;
using System.Net.Mime;

namespace PhyGen.API.Controllers
{
    [ApiController]
    [Route("api/exams")]
    public class ExamExportController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IExamExportService _exportService;
        private readonly ILogger<ExamExportController> _logger;

        public ExamExportController(
            AppDbContext db,
            IExamExportService exportService,
            ILogger<ExamExportController> logger)
        {
            _db = db;
            _exportService = exportService;
            _logger = logger;
        }

        [HttpGet("{examId:guid}/export-word")]
        //[Authorize]
        public async Task<IActionResult> ExportWord(Guid examId, CancellationToken ct)
        {
            var exam = await LoadExamAsync(examId, ct);
            if (exam == null)
                return NotFound(new { message = "Không tìm thấy đề thi." });

            var model = MapToExportModel(exam);

            var bytes = await _exportService.ExportExamToWordAsync(model, ct);
            if (bytes == null || bytes.Length == 0)
                return StatusCode(500, "Không tạo được file Word.");

            var fileName = $"{(string.IsNullOrWhiteSpace(exam.Title) ? "DE_THI" : exam.Title)}.docx";
            return File(
                fileContents: bytes,
                contentType: "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                fileDownloadName: fileName
            );
        }

        private async Task<Exam?> LoadExamAsync(Guid id, CancellationToken ct)
        {
            return await _db.Exams
               .Include(e => e.Sections)
                    .ThenInclude(s => s.QuestionSections)
                        .ThenInclude(qs => qs.Question)
                            .ThenInclude(q => q.QuestionMedias)
               .FirstOrDefaultAsync(e => e.Id == id && !e.DeletedAt.HasValue, ct);
        }

        private static ExamExportModel MapToExportModel(Exam exam)
        {
            var model = new ExamExportModel
            {
                Title = exam?.Title ?? "ĐỀ THI",
                Subject = "VẬT LÍ",                 
                Grade = exam?.Grade ?? 0,
                Year = exam?.Year ?? 0,
                Duration = "45 phút",             
                Code = string.IsNullOrWhiteSpace(exam?.ExamCode) ? "01" : exam!.ExamCode
            };

            // Sắp xếp Section theo DisplayOrder
            foreach (var s in (exam?.Sections ?? new List<Section>())
                              .OrderBy(x => x.DisplayOrder))
            {
                var sec = new SectionExportDto
                {
                    Title = s.Title ?? string.Empty,
                    SectionType = s.SectionType ?? string.Empty,
                    DisplayOrder = s.DisplayOrder
                };

                foreach (var qs in (s?.QuestionSections ?? new List<QuestionSection>()))
                {
                    var q = qs.Question;
                    if (q == null) continue;

                    var qdto = new QuestionExportDto
                    {
                        Content = q.Content ?? string.Empty,
                        Answer1 = q.Answer1,
                        Answer2 = q.Answer2,
                        Answer3 = q.Answer3,
                        Answer4 = q.Answer4,
                        Answer5 = q.Answer5,
                        Answer6 = q.Answer6,
                        Score = qs.Score,
                        ImageUrls = q.QuestionMedias?.Select(m => m.Url).Where(u => !string.IsNullOrWhiteSpace(u)).ToList()
                    };

                    sec.Questions.Add(qdto);
                }

                model.Sections.Add(sec);
            }
            return model;
        }
    }
}
