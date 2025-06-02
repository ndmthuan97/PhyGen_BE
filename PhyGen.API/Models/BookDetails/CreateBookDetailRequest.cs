using System.ComponentModel.DataAnnotations;

namespace PhyGen.API.Models.BookDetails
{
    public class CreateBookDetailRequest
    {
        [Required(ErrorMessage = "BookId is required. Format: UUID")]
        public Guid BookId { get; set; }

        [Required(ErrorMessage = "ChapterId is required. Format: UUID")]
        public Guid ChapterId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "OrderNo must be a positive number. Format: integer")]
        public int? OrderNo { get; set; }
    }
}
