using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PhyGen.Insfrastructure.Persistence.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Insfrastructure.Extensions
{
    // AppDbContextFactory class is used to create DbContext at design-time
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        // The CreateDbContext function is used to create an instance of AppDbContext with configuration from the appsettings.json file. 
        // This function will be called by EF Core when running Migration or Database related commands without starting the application.
        public AppDbContext CreateDbContext(string[] args)
        {
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "../PhyGen.API");

            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<AppDbContext>();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidDataException("DefaultConnection string is missing in appsettings.json");
            }

            builder.UseSqlServer(connectionString);

            return new AppDbContext(builder.Options);
        }
    }
}
