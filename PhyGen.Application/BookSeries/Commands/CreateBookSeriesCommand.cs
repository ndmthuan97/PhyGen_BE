using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.BookSeries.Commands
{
    public class CreateBookSeriesCommand : IRequest<Guid>
    {
        public string Name { get; set; } = string.Empty;

        public string? CreatedBy { get; set; }
    }
}
