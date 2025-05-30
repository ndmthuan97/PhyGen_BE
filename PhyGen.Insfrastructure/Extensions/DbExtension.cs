using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PhyGen.Domain.Entities;
using PhyGen.Insfrastructure.Persistence.DbContexts;
using PhyGen.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Insfrastructure.Extensions
{
    public static class DbExtension
    {
        //Register the DbContext with the connection string and additional options (if any).
        public static IServiceCollection AddDatabase<TContext>(this IServiceCollection services, string connectionString, Action<DbContextOptionsBuilder>? optionsAction = null)
        where TContext : DbContext
        {
            services.AddDbContext<TContext>(options =>
            {
                options.UseSqlServer(connectionString);
                optionsAction?.Invoke(options);
            });
            return services;
        }

        //Perform automatic database migration when the application starts.
        public static async Task MigrateDatabaseAsync<TContext>(this IApplicationBuilder app) where TContext : DbContext
        {
            using var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TContext>();
            await context.Database.MigrateAsync().ConfigureAwait(false);
        }

        //Seed default data for the Roles and Users tables (including creating users, roles, and assigning roles to users).
        public static async Task<IApplicationBuilder> SeedAuthDataAsync(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var services = scope.ServiceProvider;

            var context = services.GetRequiredService<AppDbContext>();

            await context.Database.MigrateAsync().ConfigureAwait(false);

            // Seed Users
            var usersToSeed = new List<User>
                {
                    new User
                    {
                        Id = new Guid("1a1a1a1a-1a1a-1a1a-1a1a-1a1a1a1a1a1a"),
                        Email = "thuanndmqe170240@fpt.edu.vn",
                        FirstName = "Thuan",
                        LastName = "Nguyen Dao Minh Thuan",
                        Phone = "0385908264",
                        Password = "Admin11@",
                        Role = nameof(Role.Admin),
                    },

                    new User
                    {
                        Id = new Guid("2a2a2a2a-2a2a-2a2a-2a2a-2a2a2a2a2a2a"),
                        Email = "tuyenhttqe170226@fpt.edu.vn",
                        FirstName = "Tuyen",
                        LastName = "Huynh Thi Thanh",
                        Phone = "0838683868",
                        Password = "Admin11@",
                        Role = nameof(Role.Admin),
                    },

                    new User
                    {
                        Id = new Guid("3a3a3a3a-3a3a-3a3a-3a3a-3a3a3a3a3a3a"),
                        Email = "hungntkqe170153@fpt.edu.vn",
                        FirstName = "Hung",
                        LastName = "Nguyen Tu Khanh",
                        Phone = "0838683866",
                        Password = "Admin11@",
                        Role = nameof(Role.Admin),
                    },

                    new User
                    {
                        Id = new Guid("4a4a4a4a-4a4a-4a4a-4a4a-4a4a4a4a4a4a"),
                        Email = "thinhntgqe170209@fpt.edu.vn",
                        FirstName = "Thinh",
                        LastName = "Nguyen Truong Gia",
                        Phone = "0838683866",
                        Password = "Admin11@",
                        Role = nameof(Role.Admin),
                    },

                    new User
                    {
                        Id = new Guid("5a5a5a5a-5a5a-5a5a-5a5a-5a5a5a5a5a5a"),
                        Email = "hoangngqe170225@fpt.edu.vn",
                        FirstName = "Hoang",
                        LastName = "Ngo Gia",
                        Phone = "0838683865",
                        Password = "Admin11@",
                        Role = nameof(Role.Admin),
                    }
                };

            foreach (var user in usersToSeed)
            {
                var exists = await context.Users.AnyAsync(u => u.Email == user.Email).ConfigureAwait(false);
                if (!exists)
                {
                    context.Users.Add(user);
                }
            }

            await context.SaveChangesAsync().ConfigureAwait(false);

            return app;

        }

        //Seed default data for Core Data tables (e.g. ExamCategory).
        public static async Task<IApplicationBuilder> SeedCoreDataAsync(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<AppDbContext>();

            await context.Database.MigrateAsync().ConfigureAwait(false);

            if (!await context.ExamCategories.AnyAsync().ConfigureAwait(false))
            {
                var categories = new List<ExamCategory>
                {
                    new ExamCategory { Name = "Giữa kỳ 1" },
                    new ExamCategory { Name = "Cuối kỳ 1" },
                    new ExamCategory { Name = "Giữa kỳ 2" },
                    new ExamCategory { Name = "Cuối kỳ 2" }
                };

                await context.ExamCategories.AddRangeAsync(categories).ConfigureAwait(false);
                await context.SaveChangesAsync().ConfigureAwait(false);
            }

            return app;
        }
    }
}
