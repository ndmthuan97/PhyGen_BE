using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PhyGen.API.Models
{
    public class CreateContentItemRequest
    {
        [JsonRequired]
        public Guid ContentFlowId { get; set; }
        [Required(ErrorMessage = "Trường này không được để trống.")]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "Trường này không được để trống.")]
        public string LearningOutcome { get; set; } = string.Empty;
    }
    public class UpdateContentItemRequest
    {
        [JsonRequired]
        public Guid Id { get; set; }
        [JsonRequired]
        public Guid ContentFlowId { get; set; }
        [Required(ErrorMessage = "Trường này không được để trống.")]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "Trường này không được để trống.")]
        public string LearningOutcome { get; set; } = string.Empty;
    }
}
