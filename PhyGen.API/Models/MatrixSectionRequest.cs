using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PhyGen.API.Models
{
    public class CreateMatrixSectionRequest
    {
        [JsonRequired]
        [Required(ErrorMessage = "Trường này không được để trống.")]
        public Guid MatrixId { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "Tên phần ma trận không được vượt quá 255 ký tự.")]
        public string Title { get; set; } = string.Empty;

        public double? Score { get; set; }
        public string? Description { get; set; }
    }

    public class UpdateMatrixSectionRequest
    {
        [JsonRequired]
        [Required(ErrorMessage = "Trường này không được để trống.")]
        public Guid Id { get; set; }

        [JsonRequired]
        [Required(ErrorMessage = "Trường này không được để trống.")]
        public Guid MatrixId { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "Tên phần ma trận không được vượt quá 255 ký tự.")]
        public string Title { get; set; } = string.Empty;

        public double? Score { get; set; }
        public string? Description { get; set; }
    }

    public class DeleteMatrixSectionRequest
    {
        [JsonRequired]
        [Required(ErrorMessage = "Trường này không được để trống.")]
        public Guid Id { get; set; }
    }
}
