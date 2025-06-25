using PhyGen.Shared.Constants;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PhyGen.API.Models
{
    public class CreateMatrixSectionDetailRequest
    {
        [JsonRequired]
        [Required(ErrorMessage = "Trường này không được để trống.")]
        public Guid MatrixSectionId { get; set; }

        [JsonRequired]
        [Required(ErrorMessage = "Trường này không được để trống.")]
        public Guid SectionId { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "Tiêu đề không được vượt quá 255 ký tự.")]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }
        public DifficultyLevel Level { get; set; }
        public QuestionType Type { get; set; }
        public int Quantity { get; set; }
    }

    public class UpdateMatrixSectionDetailRequest
    {
        [JsonRequired]
        [Required(ErrorMessage = "Trường này không được để trống.")]
        public Guid Id { get; set; }

        [JsonRequired]
        [Required(ErrorMessage = "Trường này không được để trống.")]
        public Guid MatrixSectionId { get; set; }

        [JsonRequired]
        [Required(ErrorMessage = "Trường này không được để trống.")]
        public Guid SectionId { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "Tiêu đề không được vượt quá 255 ký tự.")]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }
        public DifficultyLevel Level { get; set; }
        public QuestionType Type { get; set; }
        public int Quantity { get; set; }
    }

    public class DeleteMatrixSectionDetailRequest
    {
        [JsonRequired]
        [Required(ErrorMessage = "Trường này không được để trống.")]
        public Guid Id { get; set; }
    }
}
