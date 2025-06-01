using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Books.Commands
{
    public class CreateBookCommand : IRequest<Guid>
    {
        public string Name { get; set; } = string.Empty;

        public Guid? SeriesId { get; set; }

        public string? Author { get; set; }

        public int? PublicationYear { get; set; }

        public string CreatedBy { get; set; } = string.Empty;
    }
}
