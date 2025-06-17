using System.Text.Json.Serialization;

namespace PhyGen.API.Models
{
    public class CreateExamCategoryRequest
    {
        [JsonRequired]
        public string Name { get; set; } = string.Empty;
    }

    public class UpdateExamCategoryRequest
    {
        [JsonRequired]
        public Guid Id { get; set; }
        [JsonRequired]
        public string Name { get; set; } = string.Empty;
    }
}
