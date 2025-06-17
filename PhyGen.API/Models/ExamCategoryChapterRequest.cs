using System.Text.Json.Serialization;

namespace PhyGen.API.Models
{
    public class CreateExamCategoryChapterRequest
    {
        [JsonRequired]
        public Guid ExamCategoryId { get; set; }
        [JsonRequired]
        public Guid ChapterId { get; set; }
    }

    public class UpdateExamCategoryChapterRequest
    {
        [JsonRequired]
        public Guid Id { get; set; }
        [JsonRequired]
        public Guid ExamCategoryId { get; set; }
        [JsonRequired]
        public Guid ChapterId { get; set; }
    }
}
