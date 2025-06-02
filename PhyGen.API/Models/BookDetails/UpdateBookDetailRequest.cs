using System.ComponentModel.DataAnnotations;

namespace PhyGen.API.Models.BookDetails
{
    public class UpdateBookDetailRequest
    {
        [Required(ErrorMessage = "Id is required. Format: UUID")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "BookId is required. Format: UUID")]
        public Guid BookId { get; set; }

        [Required(ErrorMessage = "ChapterId is required. Format: UUID")]
        public Guid ChapterId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "OrderNo must be a positive integer.")]
        public int? OrderNo { get; set; }

    }
}
