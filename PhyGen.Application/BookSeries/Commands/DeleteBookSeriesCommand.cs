using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.BookSeries.Commands
{
    public class DeleteBookSeriesCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public string? DeletedBy { get; set; }
    }
}
