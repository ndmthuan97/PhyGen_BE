using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PhyGen.API.Models
{
    public class CreateExamCategoryRequest
    {
        [Required(ErrorMessage = "Trường này không được để trống.")]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "Trường này không được để trống.")]
        [Range(1, 50, ErrorMessage = "Số thứ tự phải lớn hơn 0.")]
        public int OrderNo { get; set; }
    }

    public class UpdateExamCategoryRequest
    {
        [JsonRequired]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Trường này không được để trống.")]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "Trường này không được để trống.")]
        [Range(1, 50, ErrorMessage = "Số thứ tự phải nằm trong khoảng từ 1 đến 50.")]
        public int OrderNo { get; set; }
    }
    public class DeleteExamCategoryRequest
    {
        [JsonRequired]
        public Guid Id { get; set; }
    }
}
