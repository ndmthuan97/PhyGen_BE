using MediatR;
using PhyGen.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.MatrixSectionDetails.Commands
{
    public class UpdateMatrixSectionDetailCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public Guid MatrixSectionId { get; set; }
        public Guid SectionId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DifficultyLevel Level { get; set; }
        public QuestionType Type { get; set; }
        public int Quantity { get; set; }
    }
}
