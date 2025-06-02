using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.BookDetails.Responses
{
    public class BookDetailResponse
    {
        public Guid Id { get; set; }

        public Guid BookId { get; set; }

        public Guid ChapterId { get; set; }

        public int? OrderNo { get; set; }
    }
}
