using System.ComponentModel.DataAnnotations;

namespace PhyGen.API.Models.Chapters
{
    public class CreateChapterRequest
    {
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 255 characters.")]
        public string Title { get; set; } = string.Empty;
        public Guid? CurriculumId { get; set; }
        public Guid? BookId { get; set; }
        public int OrderNo { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
    }
}
