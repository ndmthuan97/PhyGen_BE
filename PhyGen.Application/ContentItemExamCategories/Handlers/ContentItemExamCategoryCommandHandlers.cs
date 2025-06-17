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
    public class CreateContentItemExamCategoryCommandHandler : IRequestHandler<CreateContentItemExamCategoryCommand, Guid>
    {
        private readonly IContentItemExamCategoryRepository _contentItemExamCategoryRepository;
        private readonly IContentItemRepository _contentItemRepository;
        private readonly IExamCategoryRepository _examCategoryRepository;

        public CreateContentItemExamCategoryCommandHandler(
            IContentItemExamCategoryRepository contentItemExamCategoryRepository,
            IContentItemRepository contentItemRepository,
            IExamCategoryRepository examCategoryRepository)
        {
            _contentItemExamCategoryRepository = contentItemExamCategoryRepository;
            _contentItemRepository = contentItemRepository;
            _examCategoryRepository = examCategoryRepository;
        }

        public async Task<Guid> Handle(CreateContentItemExamCategoryCommand request, CancellationToken cancellationToken)
        {
            if (await _contentItemRepository.GetByIdAsync(request.ContentItemId) == null)
                throw new ContentItemNotFoundException();

            if (await _examCategoryRepository.GetByIdAsync(request.ExamCategoryId) == null)
                throw new ExamCategoryNotFoundException();

            if (await _contentItemExamCategoryRepository.GetContentItemExamCategoryByContentItemIdAndExamCategoryIDAsync(request.ContentItemId, request.ExamCategoryId) != null)
                throw new ContentItemExamCategoryAlreadyExistException();

            var contentItemExamCategory = new ContentItemExamCategory
            {
                ContentItemId = request.ContentItemId,
                ExamCategoryId = request.ExamCategoryId,
            };
            await _contentItemExamCategoryRepository.AddAsync(contentItemExamCategory);
            return contentItemExamCategory.Id;
        }
    }
    public class UpdateContentItemExamCategoryCommandHandler : IRequestHandler<UpdateContentItemExamCategoryCommand, Unit>
    {
        private readonly IContentItemExamCategoryRepository _contentItemExamCategoryRepository;
        private readonly IContentItemRepository _contentItemRepository;
        private readonly IExamCategoryRepository _examCategoryRepository;
        public UpdateContentItemExamCategoryCommandHandler(
            IContentItemExamCategoryRepository contentItemExamCategoryRepository,
            IContentItemRepository contentItemRepository,
            IExamCategoryRepository examCategoryRepository)
        {
            _contentItemExamCategoryRepository = contentItemExamCategoryRepository;
            _contentItemRepository = contentItemRepository;
            _examCategoryRepository = examCategoryRepository;
        }
        public async Task<Unit> Handle(UpdateContentItemExamCategoryCommand request, CancellationToken cancellationToken)
        {
            if (await _contentItemRepository.GetByIdAsync(request.ContentItemId) == null)
                throw new ContentItemNotFoundException();

            if (await _examCategoryRepository.GetByIdAsync(request.ExamCategoryId) == null)
                throw new ExamCategoryNotFoundException();

            var contentItemExamCategory = await _contentItemExamCategoryRepository.GetByIdAsync(request.Id);
            if (contentItemExamCategory == null)
                throw new ContentItemExamCategoryNotFoundException();

            contentItemExamCategory.ContentItemId = request.ContentItemId;
            contentItemExamCategory.ExamCategoryId = request.ExamCategoryId;

            await _contentItemExamCategoryRepository.UpdateAsync(contentItemExamCategory);
            return Unit.Value;
        }
    }
}
