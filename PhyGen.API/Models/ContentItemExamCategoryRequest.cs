using System.Text.Json.Serialization;

namespace PhyGen.API.Models
{
    public class CreateContentItemExamCategoryRequest
    {
        [JsonRequired]
        public Guid ContentItemId { get; set; }

        [JsonRequired]
        public Guid ExamCategoryId { get; set; }
    }

    public class UpdateContentItemExamCategoryRequest
    {
        [JsonRequired]
        public Guid Id { get; set; }
        [JsonRequired]
        public Guid ContentItemId { get; set; }
        [JsonRequired]
        public Guid ExamCategoryId { get; set; }
    }
}
