using System.ComponentModel.DataAnnotations;

namespace PhyGen.API.Models.MatrixDetail
{
    public class CreateMatrixDetailRequest
    {
        [Required]
        public Guid MatrixId { get; set; }
        [Required]
        public Guid ChapterId { get; set; }

        public Guid CreatedBy { get; set; }
    }
}
