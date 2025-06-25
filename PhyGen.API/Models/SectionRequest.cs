using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PhyGen.API.Models
{
    public class CreateSectionRequest
    {
        [JsonRequired]
        [Required(ErrorMessage = "Trường này không được để trống.")]
        public Guid ExamId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Tên phần không được vượt quá 100 ký tự.")]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }
        public string SectionType { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
    }

    public class UpdateSectionRequest
    {
        [JsonRequired]
        [Required(ErrorMessage = "Trường này không được để trống.")]
        public Guid Id { get; set; }

        [JsonRequired]
        [Required(ErrorMessage = "Trường này không được để trống.")]
        public Guid ExamId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Tên phần không được vượt quá 100 ký tự.")]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }
        public string SectionType { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
    }

    public class DeleteSectionRequest
    {
        [JsonRequired]
        [Required(ErrorMessage = "Trường này không được để trống.")]
        public Guid Id { get; set; }
    }
}
