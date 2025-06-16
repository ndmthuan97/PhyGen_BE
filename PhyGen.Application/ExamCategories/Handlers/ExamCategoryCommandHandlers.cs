using MediatR;
using PhyGen.Application.ExamCategories.Commands;
using PhyGen.Application.ExamCategories.Exceptions;
using PhyGen.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.ExamCategories.Handlers
{
    public class CreateExamCategoryCommandHandler : IRequestHandler<CreateExamCategoryCommand, int>
    {
        private readonly IExamCategoryRepository _examCategoryRepository;

        public CreateExamCategoryCommandHandler(IExamCategoryRepository examCategoryRepository)
        {
            _examCategoryRepository = examCategoryRepository;
        }

        public async Task<int> Handle(CreateExamCategoryCommand request, CancellationToken cancellationToken)
        {
            if (await _examCategoryRepository.GetByNameAsync(request.Name) != null)
                throw new ExamCategorySameNameException();

            var examCategory = new Domain.Entities.ExamCategory
            {
                Name = request.Name
            };

            await _examCategoryRepository.AddAsync(examCategory);
            return examCategory.Id;
        }
    }

    public class UpdateExamCategoryCommandHandler : IRequestHandler<UpdateExamCategoryCommand, Unit>
    {
        private readonly IExamCategoryRepository _examCategoryRepository;

        public UpdateExamCategoryCommandHandler(IExamCategoryRepository examCategoryRepository)
        {
            _examCategoryRepository = examCategoryRepository;
        }

        public async Task<Unit> Handle(UpdateExamCategoryCommand request, CancellationToken cancellationToken)
        {
            var examCategory = await _examCategoryRepository.GetByIdAsync(request.Id) ?? throw new ExamCategoryNotFoundException();

            if (await _examCategoryRepository.GetByNameAsync(request.Name) != null &&
                examCategory.Name != request.Name)
                throw new ExamCategorySameNameException();

            examCategory.Name = request.Name;

            await _examCategoryRepository.UpdateAsync(examCategory);
            return Unit.Value;
        }
    }
}
