using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Books.Commands
{
    public class UpdateBookCommand : IRequest<Unit>
    {
        public Guid BookId { get; set; }

        public string Name { get; set; } = string.Empty;

        public Guid? SeriesId { get; set; }

        public string? Author { get; set; }

        public int? PublicationYear { get; set; }

        public string UpdatedBy { get; set; } = string.Empty;
    }
}
