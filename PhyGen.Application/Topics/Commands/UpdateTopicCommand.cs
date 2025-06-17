using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Topics.Commands
{
    public class UpdateTopicCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public Guid ChapterId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int OrderNo { get; set; }
    }
}
