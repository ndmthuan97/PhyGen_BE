using MediatR;
using PhyGen.Application.Exams.Exceptions;
using PhyGen.Application.Exams.Queries;
using PhyGen.Application.Exams.Responses;
using PhyGen.Application.Mapping;
using PhyGen.Domain.Interfaces;
using PhyGen.Domain.Specs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Exams.Handlers
{
    public class GetExamByIdQueryHandler : IRequestHandler<GetExamByIdQuery, ExamResponse>
    {
        private readonly IExamRepository _examRepository;

        public GetExamByIdQueryHandler(IExamRepository examRepository)
        {
            _examRepository = examRepository;
        }

        public async Task<ExamResponse> Handle(GetExamByIdQuery request, CancellationToken cancellationToken)
        {
            var exam = await _examRepository.GetByIdAsync(request.Id)
                ?? throw new ExamNotFoundException();

            return AppMapper<CoreMappingProfile>.Mapper.Map<ExamResponse>(exam);
        }
    }

    public class GetExamsQueryHandler : IRequestHandler<GetExamsQuery, Pagination<ExamResponse>>
    {
        private readonly IExamRepository _examRepository;

        public GetExamsQueryHandler(IExamRepository examRepository)
        {
            _examRepository = examRepository;
        }

        public async Task<Pagination<ExamResponse>> Handle(GetExamsQuery request, CancellationToken cancellationToken)
        {
            var exams = await _examRepository.GetExamsAsync(request.ExamSpecParam);

            return AppMapper<CoreMappingProfile>.Mapper.Map<Pagination<ExamResponse>>(exams);
        }
    }

    public class GetExamDetailQueryHandler : IRequestHandler<GetExamDetailQuery, ExamDetailResponse>
    {
        private readonly IExamRepository _examRepository;

        public GetExamDetailQueryHandler(IExamRepository examRepository)
        {
            _examRepository = examRepository;
        }

        public async Task<ExamDetailResponse> Handle(GetExamDetailQuery request, CancellationToken cancellationToken)
        {
            var exam = await _examRepository.GetExamDetailAsync(request.ExamId);

            if (exam == null)
                throw new ExamNotFoundException();

            var response = new ExamDetailResponse
            {
                Id = exam.Id,
                Title = exam.Title,
                Description = exam.Description,
                Grade = exam.Grade,
                Year = exam.Year,
                ImgUrl = exam.ImgUrl,
                Sections = exam.Sections.OrderBy(s => s.DisplayOrder)
                    .Select(section => new SectionDetailResponse
                    {
                        Id = section.Id,
                        Title = section.Title,
                        Description = section.Description,
                        SectionType = section.SectionType,
                        DisplayOrder = section.DisplayOrder,
                        Questions = section.QuestionSections.Select(qs => new QuestionInSectionResponse
                        {
                            Id = qs.Question.Id,
                            Content = qs.Question.Content,
                            Answer1 = qs.Question.Answer1,
                            Answer2 = qs.Question.Answer2,
                            Answer3 = qs.Question.Answer3,
                            Answer4 = qs.Question.Answer4,
                            Score = qs.Score
                        }).ToList()
                    }).ToList()
            };

            return response;
        }
    }
}
