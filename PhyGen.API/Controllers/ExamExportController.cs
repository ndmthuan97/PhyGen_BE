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

        [HttpGet("{examId:guid}/export-word-multi")]
        public async Task<IActionResult> ExportWordMulti(Guid examId, [FromQuery] int versions = 1, [FromQuery] bool randomize = true, CancellationToken ct = default)
        {
            var exam = await LoadExamAsync(examId, ct);
            if (exam == null)
                return NotFound(new { message = "Không tìm thấy đề thi." });

            var model = MapToExportModel(exam);

            // Danh sách các file (tên, data)
            var files = new List<(string name, byte[] data)>();

            for (int i = 1; i <= Math.Max(1, versions); i++)
            {
                var m = randomize ? CloneAndShuffleByType(model) : model;

                // Gán Code/Mã đề khác nhau để đẩy ra header
                m.Code = i.ToString("00");

                var bytes = await _exportService.ExportExamToWordAsync(m, ct);
                if (bytes == null || bytes.Length == 0)
                    return StatusCode(500, $"Không tạo được file Word cho mã đề {i:00}.");

                var fileName = $"{(string.IsNullOrWhiteSpace(exam.Title) ? "DE_THI" : exam.Title).ToUpper()}_V{i:00}.docx";
                files.Add((fileName, bytes));
            }

            if (files.Count == 1)
            {
                // Nếu chỉ có 1 đề thì trả luôn .docx
                return File(files[0].data,
                    "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                    files[0].name);
            }

            // Nếu có nhiều đề thì trả về ZIP
            using var ms = new MemoryStream();
            using (var zip = new System.IO.Compression.ZipArchive(ms, System.IO.Compression.ZipArchiveMode.Create, true))
            {
                foreach (var f in files)
                {
                    var entry = zip.CreateEntry(f.name, System.IO.Compression.CompressionLevel.Optimal);
                    using var es = entry.Open();
                    await es.WriteAsync(f.data, 0, f.data.Length, ct);
                }
            }
            ms.Position = 0;

            var zipName = $"{(string.IsNullOrWhiteSpace(exam.Title) ? "DE_THI" : exam.Title).ToUpper()}_{files.Count}MADE.zip";
            return File(ms.ToArray(), "application/zip", zipName);
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

        private static ExamExportModel CloneAndShuffleByType(ExamExportModel model)
        {
            var rnd = new Random();
            // 1. Clone shallow model + deep copy Sections + Questions:
            var clone = new ExamExportModel
            {
                Title = model.Title,
                Subject = model.Subject,
                Grade = model.Grade,
                Year = model.Year,
                Duration = model.Duration,
                Code = model.Code,
                Sections = model.Sections.Select(s => new SectionExportDto
                {
                    Title = s.Title,
                    SectionType = s.SectionType,
                    DisplayOrder = s.DisplayOrder,
                    Questions = s.Questions?.Select(q => new QuestionExportDto
                    {
                        Content = q.Content,
                        Answer1 = q.Answer1,
                        Answer2 = q.Answer2,
                        Answer3 = q.Answer3,
                        Answer4 = q.Answer4,
                        Answer5 = q.Answer5,
                        Answer6 = q.Answer6,
                        Score = q.Score,
                        ImageUrls = q.ImageUrls != null ? new List<string>(q.ImageUrls) : null
                    }).ToList() ?? new List<QuestionExportDto>()
                }).ToList()
            };

            // 2. Gom các câu hỏi theo SectionType:
            var buckets = new Dictionary<string, List<QuestionExportDto>>();
            foreach (var s in clone.Sections)
            {
                if (string.IsNullOrWhiteSpace(s.SectionType)) continue;
                if (!buckets.TryGetValue(s.SectionType, out var list))
                {
                    list = new List<QuestionExportDto>();
                    buckets[s.SectionType] = list;
                }
                if (s.Questions != null) list.AddRange(s.Questions);
            }

            // 3. Shuffle từng bucket:
            foreach (var pair in buckets)
            {
                var list = pair.Value;
                for (int i = list.Count - 1; i > 0; i--)
                {
                    int j = rnd.Next(i + 1);
                    (list[i], list[j]) = (list[j], list[i]);
                }
            }

            // 4. Phân lại về từng section đúng số câu ban đầu:
            foreach (var s in clone.Sections)
            {
                if (string.IsNullOrWhiteSpace(s.SectionType) || s.Questions == null) continue;
                var bucket = buckets[s.SectionType];
                var count = s.Questions.Count;
                s.Questions = bucket.Take(count).ToList();
                bucket.RemoveRange(0, Math.Min(count, bucket.Count)); // lấy ra khỏi bucket
            }

            return clone;
        }
    }
}
