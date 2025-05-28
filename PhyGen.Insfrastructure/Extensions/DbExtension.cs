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
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
            var userManager = services.GetRequiredService<UserManager<User>>();

            await context.Database.MigrateAsync().ConfigureAwait(false);

            // Seed Roles
            await SeedRolesAsync(roleManager, nameof(Role.Admin), nameof(Role.User)).ConfigureAwait(false);

            // Seed Users
            var usersToSeed = new List<(User user, string password, string role)>
                {
                    (new User
                    {
                        Id = new Guid("1a1a1a1a-1a1a-1a1a-1a1a-1a1a1a1a1a1a"),
                        Email = "thuanndmqe170240@fpt.edu.vn",
                        UserName = "thuanndmqe170240@fpt.edu.vn",
                        EmailConfirmed = true,
                        FirstName = "Thuan",
                        LastName = "Nguyen Dao Minh Thuan",
                        PhoneNumber = "0385908264"
                    }, "Admin11@", nameof(Role.Admin)),

                    (new User
                    {
                        Id = new Guid("2a2a2a2a-2a2a-2a2a-2a2a-2a2a2a2a2a2a"),
                        Email = "tuyenhttqe170226@fpt.edu.vn",
                        UserName = "tuyenhttqe170226@fpt.edu.vn",
                        EmailConfirmed = true,
                        FirstName = "Tuyen",
                        LastName = "Huynh Thi Thanh",
                        PhoneNumber = "0838683868"
                    }, "User111@", nameof(Role.Admin)),

                    (new User
                    {
                        Id = new Guid("3a3a3a3a-3a3a-3a3a-3a3a-3a3a3a3a3a3a"),
                        Email = "hungntkqe170153@fpt.edu.vn",
                        UserName = "hungntkqe170153@fpt.edu.vn",
                        EmailConfirmed = true,
                        FirstName = "Hung",
                        LastName = "Nguyen Tu Khanh",
                        PhoneNumber = "0838683866"
                    }, "User111@", nameof(Role.Admin)),

                    (new User
                    {
                        Id = new Guid("4a4a4a4a-4a4a-4a4a-4a4a-4a4a4a4a4a4a"),
                        Email = "thinhntgqe170209@fpt.edu.vn",
                        UserName = "thinhntgqe170209@fpt.edu.vn",
                        EmailConfirmed = true,
                        FirstName = "Thinh",
                        LastName = "Nguyen Truong Gia",
                        PhoneNumber = "0838683866"
                    }, "User111@", nameof(Role.Admin)),

                    (new User
                    {
                        Id = new Guid("5a5a5a5a-5a5a-5a5a-5a5a-5a5a5a5a5a5a"),
                        Email = "hoangngqe170225@fpt.edu.vn",
                        UserName = "hoangngqe170225@fpt.edu.vn",
                        EmailConfirmed = true,
                        FirstName = "Hoang",
                        LastName = "Ngo Gia",
                        PhoneNumber = "0838683865"
                    }, "User111@", nameof(Role.Admin))
                };

            if (!await context.Users.AnyAsync().ConfigureAwait(false))
            {
                foreach (var (user, password, role) in usersToSeed)
                {
                    var userExists = await userManager.FindByEmailAsync(user.Email);
                    if (userExists == null)
                    {
                        var result = await userManager.CreateAsync(user, password).ConfigureAwait(false);
                        if (result.Succeeded)
                        {
                            var roleResult = await userManager.AddToRoleAsync(user, role).ConfigureAwait(false);
                            if (!roleResult.Succeeded)
                            {
                                throw new Exception($"Failed to add user {user.Email} to role {role}: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
                            }
                        }
                        else
                        {
                            throw new Exception($"Failed to create user {user.Email}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                        }
                    }
                }
            }

            return app;
        }

        //Create roles if they do not exist in the database.
        private static async Task SeedRolesAsync(RoleManager<IdentityRole<Guid>> roleManager, params string[] roles)
        {
            foreach (var role in roles)
            {
                var roleExists = await roleManager.RoleExistsAsync(role).ConfigureAwait(false);
                if (!roleExists)
                {
                    var createResult = await roleManager.CreateAsync(new IdentityRole<Guid>(role)).ConfigureAwait(false);
                    if (!createResult.Succeeded)
                    {
                        throw new Exception($"Failed to create role {role}: {string.Join(", ", createResult.Errors.Select(e => e.Description))}");
                    }
                }
            }
        }

        //Create sample users if there are no users in the database and assign corresponding roles.
        private static async Task SeedUsersAsync(UserManager<User> userManager, AppDbContext context, List<(User user, string password, string role)> usersToSeed)
        {
            if (!await context.Users.AnyAsync().ConfigureAwait(false))
            {
                foreach (var (user, password, role) in usersToSeed)
                {
                    var result = await userManager.CreateAsync(user, password).ConfigureAwait(false);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, role).ConfigureAwait(false);
                    }
                    else
                    {
                        throw new Exception($"Failed to create user {user.Email}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    }
                }
            }
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
