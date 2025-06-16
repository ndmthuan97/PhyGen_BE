using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PhyGen.API.Models
{
    public class CreateMatrixRequest
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 255 characters.")]
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
        public string Grade { get; set; } = string.Empty;

        [JsonRequired]
        public Guid SubjectId { get; set; }

        [JsonRequired]
        public Guid ExamCategoryId { get; set; }

        public string CreatedBy { get; set; } = string.Empty;
    }

    public class UpdateMatrixRequest
    {
        [JsonRequired]
        public Guid MatrixId { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 255 characters.")]
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
        public string Grade { get; set; } = string.Empty;

        [JsonRequired]
        public Guid SubjectId { get; set; }

        [JsonRequired]
        public Guid ExamCategoryId { get; set; }

        public string UpdatedBy { get; set; } = string.Empty;
    }

    public class DeleteMatrixRequest
    {
        [Required(ErrorMessage = "MatrixId is required.")]
        public Guid MatrixId { get; set; }

        public string DeletedBy { get; set; } = string.Empty;
    }
}
