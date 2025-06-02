using System.ComponentModel.DataAnnotations;

namespace PhyGen.API.Models.MatrixDetails
{
    public class UpdateMatrixDetailRequest
    {
        [Required]
        public Guid MatrixId { get; set; }
        [Required]
        public Guid ChapterId { get; set; }

        public Guid UpdatedBy { get; set; }
    }
}
