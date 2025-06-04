using System.Text.Json.Serialization;

namespace PhyGen.API.Models.Questions
{
    public class UpdateQuestionRequest
    {
        public Guid Id { get; set; }

        public string Content { get; set; } = string.Empty;

        [JsonRequired]
        public string UpdatedBy { get; set; } = string.Empty;

        //public string Answer { get; set; } = string.Empty;
    }
}
