using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.BookDetails.Commands
{
    public class CreateBookDetailCommand : IRequest<Guid>
    {
        public Guid BookId { get; set; }

        public Guid ChapterId { get; set; }

        public int? OrderNo { get; set; }
    }
}
