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
    public class BookRepository : RepositoryBase<Book, Guid>, IBookRepository
    {
        public BookRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Book?> GetBookByNameAsync(string bookName)
        {
            return  await _context.Books
                .FirstOrDefaultAsync(b => b.Name.ToLower() == bookName.ToLower());
        }

        public async Task<List<Book>> GetBooksByBookSeriesIdAsync(Guid bookSeriesId)
        {
            return await _context.Books
                .Where(b => b.SeriesId == bookSeriesId)
                .ToListAsync();
        }
    }
}
