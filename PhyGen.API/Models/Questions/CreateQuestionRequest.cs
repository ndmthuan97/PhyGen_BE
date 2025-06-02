using System.Text.Json.Serialization;

namespace PhyGen.API.Models.Questions
{
    public class CreateQuestionRequest
    {
        public string Content { get; set; } = string.Empty;

        [JsonRequired]
        public string CreatedBy { get; set; } = string.Empty;

        //public string Answer { get; set; } = string.Empty;
    }
}
