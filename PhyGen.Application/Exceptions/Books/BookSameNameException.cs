using PhyGen.Domain.Exceptions;
using PhyGen.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Exceptions.Books
{
    public class BookSameNameException : AppException
    {
        public BookSameNameException() : base(StatusCode.BookSameName)
        {
        }
    }
}
