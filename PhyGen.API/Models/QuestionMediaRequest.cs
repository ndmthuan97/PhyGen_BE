using System.Text.Json.Serialization;

namespace PhyGen.API.Models
{
    public class CreateQuestionMediaRequest
    {
        [JsonRequired]
        public Guid QuestionId { get; set; }
     
        public string MediaType { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
    }

    public class UpdateQuestionMediaRequest
    {
        [JsonRequired]
        public Guid QuestionMediaId { get; set; }
        
        [JsonRequired]
        public Guid QuestionId { get; set; }
        
        public string MediaType { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
    }
}
