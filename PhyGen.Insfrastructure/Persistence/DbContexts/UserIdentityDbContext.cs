using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PhyGen.Insfrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Insfrastructure.Persistence.DbContexts
{
    public class UserIdentityDbContext : IdentityDbContext<Customer>
    {
        protected UserIdentityDbContext()
        {
        }

        public UserIdentityDbContext(DbContextOptions<UserIdentityDbContext> options) : base(options)
        {
        }
    }
}
