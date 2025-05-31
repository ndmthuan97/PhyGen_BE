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
    public class BookSeriesRepository : RepositoryBase<BookSeries, Guid>, IBookSeriesRepository
    {
        public BookSeriesRepository(AppDbContext context) : base(context)
        {
        }
    }
}
