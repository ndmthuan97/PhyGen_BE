using Microsoft.EntityFrameworkCore;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Interfaces.Repositories;
using PhyGen.Insfrastructure.Persistence.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Insfrastructure.Persistence.Repositories
{
    public class BookDetailRepository : RepositoryBase<BookDetail, Guid>, IBookDetailRepository
    {
        public BookDetailRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<BookDetail?> GetBookDetailByBookIdAndChapterIdAsync(Guid bookId, Guid chapterId)
        {
            return await _context.BookDetails
                .FirstOrDefaultAsync(bd => bd.BookId == bookId && bd.ChapterId == chapterId);
        }
    }
}
