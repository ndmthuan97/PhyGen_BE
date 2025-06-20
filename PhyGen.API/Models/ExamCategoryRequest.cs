using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PhyGen.API.Models
{
    public class CreateExamCategoryRequest
    {
        [Required(ErrorMessage = "Trường này không được để trống.")]
        public string Name { get; set; } = string.Empty;
    }

    public class UpdateExamCategoryRequest
    {
        [JsonRequired]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Trường này không được để trống.")]
        public string Name { get; set; } = string.Empty;
    }
}
