using System.ComponentModel.DataAnnotations;

namespace PhyGen.API.Models.Exam
{
    public class CreateExamRequest
    {
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 255 characters.")]

        public string Title { get; set; } = string.Empty;

        public Guid CreatedBy { get; set; }
    }
}
