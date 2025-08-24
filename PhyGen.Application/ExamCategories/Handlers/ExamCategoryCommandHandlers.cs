using MediatR;
using PhyGen.Application.ExamCategories.Commands;
using PhyGen.Application.ExamCategories.Exceptions;
using PhyGen.Application.ExamCategories.Responses;
using PhyGen.Application.Mapping;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.ExamCategories.Handlers
{
    public class CreateExamCategoryCommandHandler : IRequestHandler<CreateExamCategoryCommand, ExamCategoryResponse>
    {
        private readonly IExamCategoryRepository _examCategoryRepository;

        public CreateExamCategoryCommandHandler(IExamCategoryRepository examCategoryRepository)
        {
            _examCategoryRepository = examCategoryRepository;
        }

        public async Task<ExamCategoryResponse> Handle(CreateExamCategoryCommand request, CancellationToken cancellationToken)
        {
            if (await _examCategoryRepository.AlreadyExistAsync(ec => 
                ec.Name.ToLower() == request.Name.ToLower() &&
                ec.DeletedAt == null
                ))
                throw new ExamCategorySameNameException();

            var examCategory = new ExamCategory
            {
                Name = request.Name,
                OrderNo = await _examCategoryRepository.GetOrderNoMaxAsync() + 1
            };

            await _examCategoryRepository.AddAsync(examCategory);
            return AppMapper<CoreMappingProfile>.Mapper.Map<ExamCategoryResponse>(examCategory);
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
            var examCategory = await _examCategoryRepository.GetByIdAsync(request.Id);
            if (examCategory == null || examCategory.DeletedAt.HasValue)
                throw new ExamCategoryNotFoundException();

            if (await _examCategoryRepository.AlreadyExistAsync(ec => 
                ec.Name.ToLower() == request.Name.ToLower() &&
                ec.DeletedAt == null
                ))
                throw new ExamCategorySameNameException();

            examCategory.Name = request.Name;

            await _examCategoryRepository.UpdateAsync(examCategory);
            return Unit.Value;
        }
    }
    public class DeleteExamCategoryCommandHandler : IRequestHandler<DeleteExamCategoryCommand, Unit>
    {
        private readonly IExamCategoryRepository _examCategoryRepository;
        public DeleteExamCategoryCommandHandler(IExamCategoryRepository examCategoryRepository)
        {
            _examCategoryRepository = examCategoryRepository;
        }
        public async Task<Unit> Handle(DeleteExamCategoryCommand request, CancellationToken cancellationToken)
        {
            var examCategory = await _examCategoryRepository.GetByIdAsync(request.Id);

            if (examCategory == null || examCategory.DeletedAt.HasValue)
                throw new ExamCategoryNotFoundException();

            examCategory.DeletedAt = DateTime.UtcNow;

            await _examCategoryRepository.UpdateAsync(examCategory);
            return Unit.Value;
        }
    }
}
