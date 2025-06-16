using MediatR;
using PhyGen.Application.ExamCategories.Exceptions;
using PhyGen.Application.ExamCategories.Queries;
using PhyGen.Application.ExamCategories.Responses;
using PhyGen.Application.Mapping;
using PhyGen.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.ExamCategories.Handlers
{
    public class GetAllExamCategoriesQueryHandler : IRequestHandler<GetAllExamCategoriesQuery, List<ExamCategoryResponse>>
    {
        private readonly IExamCategoryRepository _examCategoryRepository;

        public GetAllExamCategoriesQueryHandler(IExamCategoryRepository examCategoryRepository)
        {
            _examCategoryRepository = examCategoryRepository;
        }

        public async Task<List<ExamCategoryResponse>> Handle(GetAllExamCategoriesQuery request, CancellationToken cancellationToken)
        {
            var examCategories = await _examCategoryRepository.GetAllAsync();
            
            return AppMapper<CoreMappingProfile>.Mapper.Map<List<ExamCategoryResponse>>(examCategories);
        }
    }

    public class GetExamCategoryByIdQueryHandler : IRequestHandler<GetExamCategoryByIdQuery, ExamCategoryResponse>
    {
        private readonly IExamCategoryRepository _examCategoryRepository;

        public GetExamCategoryByIdQueryHandler(IExamCategoryRepository examCategoryRepository)
        {
            _examCategoryRepository = examCategoryRepository;
        }

        public async Task<ExamCategoryResponse> Handle(GetExamCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var examCategory = await _examCategoryRepository.GetByIdAsync(request.Id)
                ?? throw new ExamCategoryNotFoundException();

            return AppMapper<CoreMappingProfile>.Mapper.Map<ExamCategoryResponse>(examCategory);
        }
    }
}
