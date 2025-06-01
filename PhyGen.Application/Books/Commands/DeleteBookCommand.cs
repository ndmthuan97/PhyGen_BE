using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Books.Commands
{
    public class DeleteBookCommand : IRequest<Unit>
    {
        public Guid BookId { get; set; }
        public string DeletedBy { get; set; } = string.Empty;
    }
}
