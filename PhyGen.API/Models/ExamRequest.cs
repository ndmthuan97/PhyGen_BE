using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PhyGen.API.Models
{
    public class CreateExamRequest
    {
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 255 characters.")]
        public string Title { get; set; } = string.Empty;

        [JsonRequired]
        public Guid MatrixId { get; set; }
        [JsonRequired]
        public int CategoryId { get; set; }
        [JsonRequired]
        public Guid SubjectCurriculumId { get; set; }
     
        public string CreatedBy { get; set; } = string.Empty;
    }

    public class UpdateExamRequest
    {
        [JsonRequired]
        public Guid ExamId { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 255 characters.")]
        public string Title { get; set; } = string.Empty;

        [JsonRequired]
        public Guid MatrixId { get; set; }
        [JsonRequired]
        public int CategoryId { get; set; }
        [JsonRequired]
        public Guid SubjectCurriculumId { get; set; }

        public string UpdatedBy { get; set; } = string.Empty;
    }

    public class DeleteExamRequest
    {
        [Required(ErrorMessage = "ExamId is required.")]
        public Guid ExamId { get; set; }

        public string DeletedBy { get; set; } = string.Empty;
    }
}
