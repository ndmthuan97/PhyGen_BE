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
        [Required(ErrorMessage = "Trường này không được để trống.")]
        [Range(1, int.MaxValue, ErrorMessage = "Số thứ tự phải lớn hơn 0.")]
        public int OrderNo { get; set; }
        [Required(ErrorMessage = "Trường này không được để trống.")]
        [Range(10, 12, ErrorMessage = "Lớp học phải từ 10 đến 12.")]
        public int Grade { get; set; }
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
        [Required(ErrorMessage = "Trường này không được để trống.")]
        [Range(1, int.MaxValue, ErrorMessage = "Số thứ tự phải lớn hơn 0.")]
        public int OrderNo { get; set; }
        [Required(ErrorMessage = "Trường này không được để trống.")]
        [Range(10, 12, ErrorMessage = "Lớp học phải từ 10 đến 12.")]
        public int Grade { get; set; }
    }
    public class DeleteContentFlowRequest
    {
        [JsonRequired]
        public Guid Id { get; set; }
    }
}
