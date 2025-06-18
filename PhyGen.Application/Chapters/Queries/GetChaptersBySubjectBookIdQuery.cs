using MediatR;
using PhyGen.Application.Chapters.Responses;
using PhyGen.Domain.Specs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Chapters.Queries
{
    public record GetChaptersBySubjectBookIdQuery(ChapterSpecParam ChapterSpecParam) : IRequest<Pagination<ChapterResponse>>;
}
