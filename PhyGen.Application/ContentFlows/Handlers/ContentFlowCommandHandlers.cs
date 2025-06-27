using MediatR;
using PhyGen.Application.ContentFlows.Commands;
using PhyGen.Application.ContentFlows.Exceptions;
using PhyGen.Application.ContentFlows.Responses;
using PhyGen.Application.Curriculums.Exceptions;
using PhyGen.Application.Mapping;
using PhyGen.Application.Subjects.Exceptions;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.ContentFlows.Handlers
{
    public class CreateContentFlowCommandHandler : IRequestHandler<CreateContentFlowCommand, ContentFlowResponse>
    {
        private readonly IContentFlowRepository _repository;
        private readonly ICurriculumRepository _curriculumRepository;
        private readonly ISubjectRepository _subjectRepository;

        public CreateContentFlowCommandHandler(IContentFlowRepository repository, ICurriculumRepository curriculumRepository, ISubjectRepository subjectRepository)
        {
            _repository = repository;
            _curriculumRepository = curriculumRepository;
            _subjectRepository = subjectRepository;
        }

        public async Task<ContentFlowResponse> Handle(CreateContentFlowCommand request, CancellationToken cancellationToken)
        {
            var curriculum = await _curriculumRepository.GetByIdAsync(request.CurriculumId);
            if (curriculum == null || curriculum.DeletedAt.HasValue)
                throw new CurriculumNotFoundException();

            if (await _subjectRepository.GetByIdAsync(request.SubjectId) == null)
                throw new SubjectNotFoundException();

            if (await _repository.AlreadyExistAsync(cf =>
                cf.CurriculumId == request.CurriculumId &&
                cf.SubjectId == request.SubjectId &&
                cf.Name.ToLower() == request.Name.ToLower() &&
                cf.Description.ToLower() == request.Description.ToLower() &&
                cf.OrderNo == request.OrderNo &&
                cf.Grade == request.Grade &&
                cf.DeletedAt == null
                ))
                throw new ContentFlowAlreadyExistException();

            var contentFlow = new ContentFlow
            {
                CurriculumId = request.CurriculumId,
                SubjectId = request.SubjectId,
                Name = request.Name,
                Description = request.Description,
                OrderNo = request.OrderNo,
                Grade = request.Grade
            };

            await _repository.AddAsync(contentFlow);
            return AppMapper<CoreMappingProfile>.Mapper.Map<ContentFlowResponse>(contentFlow);
        }
    }

    public class UpdateContentFlowCommandHandler : IRequestHandler<UpdateContentFlowCommand, Unit>
    {
        private readonly IContentFlowRepository _repository;
        private readonly ICurriculumRepository _curriculumRepository;
        private readonly ISubjectRepository _subjectRepository;
        public UpdateContentFlowCommandHandler(IContentFlowRepository repository, ICurriculumRepository curriculumRepository, ISubjectRepository subjectRepository)
        {
            _repository = repository;
            _curriculumRepository = curriculumRepository;
            _subjectRepository = subjectRepository;
        }
        public async Task<Unit> Handle(UpdateContentFlowCommand request, CancellationToken cancellationToken)
        {
            var contentFlow = await _repository.GetByIdAsync(request.Id);
            if (contentFlow == null || contentFlow.DeletedAt.HasValue)
                throw new ContentFlowNotFoundException();

            var curriculum = await _curriculumRepository.GetByIdAsync(request.CurriculumId);
            if (curriculum == null || curriculum.DeletedAt.HasValue)
                throw new CurriculumNotFoundException();

            if (await _subjectRepository.GetByIdAsync(request.SubjectId) == null)
                throw new SubjectNotFoundException();

            if (await _repository.AlreadyExistAsync(cf =>
                cf.CurriculumId == request.CurriculumId &&
                cf.SubjectId == request.SubjectId &&
                cf.Name.ToLower() == request.Name.ToLower() &&
                cf.Description.ToLower() == request.Description.ToLower() &&
                cf.OrderNo == request.OrderNo &&
                cf.Grade == request.Grade &&
                cf.DeletedAt == null
                ))
                throw new ContentFlowAlreadyExistException();

            contentFlow.CurriculumId = request.CurriculumId;
            contentFlow.SubjectId = request.SubjectId;
            contentFlow.Name = request.Name;
            contentFlow.Description = request.Description;
            contentFlow.OrderNo = request.OrderNo;
            contentFlow.Grade = request.Grade;

            await _repository.UpdateAsync(contentFlow);
            return Unit.Value;
        }
    }
    public class DeleteContentFlowCommandHandler : IRequestHandler<DeleteContentFlowCommand, Unit>
    {
        private readonly IContentFlowRepository _repository;
        public DeleteContentFlowCommandHandler(IContentFlowRepository repository)
        {
            _repository = repository;
        }
        public async Task<Unit> Handle(DeleteContentFlowCommand request, CancellationToken cancellationToken)
        {
            var contentFlow = await _repository.GetByIdAsync(request.Id);
            if (contentFlow == null || contentFlow.DeletedAt.HasValue)
                throw new ContentFlowNotFoundException();

            contentFlow.DeletedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(contentFlow);
            return Unit.Value;
        }
    }
}
