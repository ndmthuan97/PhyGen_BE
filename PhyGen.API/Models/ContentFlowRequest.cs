using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PhyGen.API.Models
{
    public class CreateContentFlowRequest
    {
        [JsonRequired]
        public Guid CurriculumId { get; set; }
        [JsonRequired]
        public Guid SubjectId { get; set; }
        [Required(ErrorMessage = "Trường này không được để trống.")]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class UpdateContentFlowRequest
    {
        [JsonRequired]
        public Guid Id { get; set; }
        [JsonRequired]
        public Guid CurriculumId { get; set; }
        [JsonRequired]
        public Guid SubjectId { get; set; }
        [Required(ErrorMessage = "Trường này không được để trống.")]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
