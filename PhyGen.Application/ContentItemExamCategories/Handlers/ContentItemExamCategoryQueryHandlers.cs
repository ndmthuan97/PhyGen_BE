using MediatR;
using PhyGen.Application.ContentItemExamCategories.Exceptions;
using PhyGen.Application.ContentItemExamCategories.Queries;
using PhyGen.Application.ContentItemExamCategories.Responses;
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
        private readonly IContentItemExamCategoryRepository _repository;

        public GetContentItemExamCategoryByIdQueryHandler(IContentItemExamCategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<ContentItemExamCategoryResponse> Handle(GetContentItemExamCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var contentItemExamCategory = await _repository.GetByIdAsync(request.Id) ?? throw new ContentItemExamCategoryNotFoundException();

            return AppMapper<CoreMappingProfile>.Mapper.Map<ContentItemExamCategoryResponse>(contentItemExamCategory);
        }
    }

    public class GetContentItemExamCategoryByContentItemIdAndExamCategoryIdQueryHandler : IRequestHandler<GetContentItemExamCategoryByContentItemIdAndExamCategoryIdQuery, ContentItemExamCategoryResponse>
    {
        private readonly IContentItemExamCategoryRepository _repository;

        public GetContentItemExamCategoryByContentItemIdAndExamCategoryIdQueryHandler(IContentItemExamCategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<ContentItemExamCategoryResponse> Handle(GetContentItemExamCategoryByContentItemIdAndExamCategoryIdQuery request, CancellationToken cancellationToken)
        {
            var contentItemExamCategory = await _repository.GetByContentItemIdAndExamCategoryIdAsync(request.ContentItemId, request.ExamCategoryId)
                ?? throw new ContentItemExamCategoryNotFoundException();

            return AppMapper<CoreMappingProfile>.Mapper.Map<ContentItemExamCategoryResponse>(contentItemExamCategory);
        }
    }
}
