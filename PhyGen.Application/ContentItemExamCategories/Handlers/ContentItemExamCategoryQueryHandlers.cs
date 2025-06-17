using MediatR;
using PhyGen.Application.ContentItemExamCategories.Exceptions;
using PhyGen.Application.ContentItemExamCategories.Queries;
using PhyGen.Application.ContentItemExamCategories.Responses;
using PhyGen.Application.ContentItems.Exceptions;
using PhyGen.Application.ExamCategories.Exceptions;
using PhyGen.Application.Mapping;
using PhyGen.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.ContentItemExamCategories.Handlers
{
    public class GetContentItemExamCategoryByIdQueryHandler : IRequestHandler<GetContentItemExamCategoryByIdQuery, ContentItemExamCategoryResponse>
    {
        private readonly IContentItemExamCategoryRepository _contentItemExamCategoryRepository;
        public GetContentItemExamCategoryByIdQueryHandler(IContentItemExamCategoryRepository contentItemExamCategoryRepository)
        {
            _contentItemExamCategoryRepository = contentItemExamCategoryRepository;
        }
        public async Task<ContentItemExamCategoryResponse> Handle(GetContentItemExamCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var contentItemExamCategory = await _contentItemExamCategoryRepository.GetByIdAsync(request.Id) ?? throw new ContentItemExamCategoryNotFoundException();
           
            return AppMapper<CoreMappingProfile>.Mapper.Map<ContentItemExamCategoryResponse>(contentItemExamCategory);
        }
    }

    public class GetContentItemExamCategoryByContentItemIdAndExamCategoryIdQueryHandler : IRequestHandler<GetContentItemExamCategoryByContentItemIdAndExamCategoryIdQuery, ContentItemExamCategoryResponse>
    {
        private readonly IContentItemExamCategoryRepository _contentItemExamCategoryRepository;
        private readonly IContentItemRepository _contentItemRepository;
        private readonly IExamCategoryRepository _examCategoryRepository;

        public GetContentItemExamCategoryByContentItemIdAndExamCategoryIdQueryHandler(
            IContentItemExamCategoryRepository contentItemExamCategoryRepository,
            IContentItemRepository contentItemRepository,
            IExamCategoryRepository examCategoryRepository)
        {
            _contentItemExamCategoryRepository = contentItemExamCategoryRepository;
            _contentItemRepository = contentItemRepository;
            _examCategoryRepository = examCategoryRepository;
        }

        public async Task<ContentItemExamCategoryResponse> Handle(GetContentItemExamCategoryByContentItemIdAndExamCategoryIdQuery request, CancellationToken cancellationToken)
        {
            if (await _contentItemRepository.GetByIdAsync(request.ContentItemId) == null)
                throw new ContentItemNotFoundException();

            if (await _examCategoryRepository.GetByIdAsync(request.ExamCategoryId) == null)
                throw new ExamCategoryNotFoundException();

            var contentItemExamCategories = await _contentItemExamCategoryRepository.GetContentItemExamCategoryByContentItemIdAndExamCategoryIDAsync(request.ContentItemId, request.ExamCategoryId) ?? throw new ContentItemExamCategoryNotFoundException();

            return AppMapper<CoreMappingProfile>.Mapper.Map<ContentItemExamCategoryResponse>(contentItemExamCategories);
        }
    }
}
