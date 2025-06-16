using System.Text.Json.Serialization;

namespace PhyGen.API.Models
{
    public class CreateExamQuestionRequest
    {
        [JsonRequired]
        public Guid ExamId { get; set; }

        [JsonRequired]
        public Guid QuestionId { get; set; }
    }

    public class UpdateExamQuestionRequest
    {
        [JsonRequired]
        public Guid ExamQuestionId { get; set; }

        [JsonRequired]
        public Guid ExamId { get; set; }

        [JsonRequired]
        public Guid QuestionId { get; set; }
    }
}
