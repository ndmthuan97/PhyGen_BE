using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PhyGen.API.Models
{
    public class CreateCurriculumRequest
    {
        [Required(ErrorMessage = "Trường này không được để trống.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Trường này không được để trống.")]
        public int Grade { get; set; }
    }

    public class UpdateCurriculumRequest
    {
        [JsonRequired]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Trường này không được để trống.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Trường này không được để trống.")]
        public int Grade { get; set; }
    }

    public class DeleteCurriculumRequest
    {
        [JsonRequired]
        public Guid Id { get; set; }
    }
}
