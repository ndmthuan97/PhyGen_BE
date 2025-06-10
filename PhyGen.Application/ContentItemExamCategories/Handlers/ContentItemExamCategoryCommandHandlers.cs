using MediatR;
using PhyGen.Application.ContentItemExamCategories.Commands;
using PhyGen.Application.ContentItemExamCategories.Exceptions;
using PhyGen.Application.ContentItems.Exceptions;
using PhyGen.Application.ExamCategories.Exceptions;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.ContentItemExamCategories.Handlers
{
    public class CreateContentItemExamCategoryCommandHandler : IRequestHandler<CreateContentItemExamCategoryCommand, int>
    {
        private readonly IContentItemExamCategoryRepository _repository;
        private readonly IContentItemRepository _contentItemRepository;
        private readonly IExamCategoryRepository _examCategoryRepository;

        public CreateContentItemExamCategoryCommandHandler(
            IContentItemExamCategoryRepository repository,
            IContentItemRepository contentItemRepository,
            IExamCategoryRepository examCategoryRepository)
        {
            _repository = repository;
            _contentItemRepository = contentItemRepository;
            _examCategoryRepository = examCategoryRepository;
        }

        public async Task<int> Handle(CreateContentItemExamCategoryCommand request, CancellationToken cancellationToken)
        {
            if (await _contentItemRepository.GetByIdAsync(request.ContentItemId) == null)
                throw new ContentItemNotFoundException();

            if (await _examCategoryRepository.GetByIdAsync(request.ExamCategoryId) == null)
                throw new ExamCategoryNotFoundException();

            if (await _repository.GetByContentItemIdAndExamCategoryIdAsync(request.ContentItemId, request.ExamCategoryId) != null)
                throw new ContentItemExamCategoryAlreadyExistException();

            var contentItemExamCategory = new ContentItemExamCategory
            {
                ContentItemId = request.ContentItemId,
                ExamCategoryId = request.ExamCategoryId,
            };

            await _repository.AddAsync(contentItemExamCategory);
            return contentItemExamCategory.Id;
        }
    }

    public class UpdateContentItemExamCategoryCommandHandler : IRequestHandler<UpdateContentItemExamCategoryCommand, Unit>
    {
        private readonly IContentItemExamCategoryRepository _repository;
        private readonly IContentItemRepository _contentItemRepository;
        private readonly IExamCategoryRepository _examCategoryRepository;
        public UpdateContentItemExamCategoryCommandHandler(
            IContentItemExamCategoryRepository repository,
            IContentItemRepository contentItemRepository,
            IExamCategoryRepository examCategoryRepository)
        {
            _repository = repository;
            _contentItemRepository = contentItemRepository;
            _examCategoryRepository = examCategoryRepository;
        }
        public async Task<Unit> Handle(UpdateContentItemExamCategoryCommand request, CancellationToken cancellationToken)
        {
            var contentItemExamCategory = await _repository.GetByIdAsync(request.Id) ?? throw new ContentItemExamCategoryNotFoundException();
            
            if (await _contentItemRepository.GetByIdAsync(request.ContentItemId) == null)
                throw new ContentItemNotFoundException();
            
            if (await _examCategoryRepository.GetByIdAsync(request.ExamCategoryId) == null)
                throw new ExamCategoryNotFoundException();

            contentItemExamCategory.ContentItemId = request.ContentItemId;
            contentItemExamCategory.ExamCategoryId = request.ExamCategoryId;

            await _repository.UpdateAsync(contentItemExamCategory);
            return Unit.Value;
        }
    }
}
