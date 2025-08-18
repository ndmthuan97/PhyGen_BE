using MediatR;
using PhyGen.Shared.Constants;

namespace PhyGen.Application.Matrices.Commands
{
    public class UpdateMatrixFullCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int TotalQuestionCount { get; set; }
        public int Grade { get; set; }
        public int Year { get; set; }
        public string? ImgUrl { get; set; }
        public StatusQEM Status { get; set; }
        public string? MatrixCode { get; set; } = string.Empty;

        public List<MatrixSectionDto> Sections { get; set; } = new();
    }

    public class MatrixSectionDto
    {
        public Guid? Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public double? Score { get; set; }
        public string? Description { get; set; }
        public List<MatrixSectionDetailDto> Details { get; set; } = new();
    }

    public class MatrixSectionDetailDto
    {
        public Guid? Id { get; set; }
        public Guid MatrixSectionId { get; set; }
        public Guid ContentItemId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DifficultyLevel Level { get; set; }
        public QuestionType Type { get; set; }
        public int Quantity { get; set; }
    }
}
