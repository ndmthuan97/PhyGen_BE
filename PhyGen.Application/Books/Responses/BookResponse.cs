using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Books.Responses
{
    public class BookResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public Guid? SeriesId { get; set; }

        public string? Author { get; set; }

        public int? PublicationYear { get; set; }
    }
}
