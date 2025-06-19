using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PhyGen.Domain.Entities;
using PhyGen.Insfrastructure.Persistence.DbContexts;
using PhyGen.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Insfrastructure.Extensions
{
    public static class DbExtension
    {
        // Đăng ký DbContext sử dụng PostgreSQL
        public static IServiceCollection AddDatabase<TContext>(this IServiceCollection services, string connectionString, Action<DbContextOptionsBuilder>? optionsAction = null)
            where TContext : DbContext
        {
            services.AddDbContext<TContext>(options =>
            {
                options.UseNpgsql(connectionString);
                optionsAction?.Invoke(options);
            });

            return services;
        }

        // Tự động migrate khi ứng dụng khởi động
        public static async Task MigrateDatabaseAsync<TContext>(this IApplicationBuilder app) where TContext : DbContext
        {
            using var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TContext>();
            await context.Database.MigrateAsync().ConfigureAwait(false);
        }

        // Seed dữ liệu người dùng mặc định
        public static async Task<IApplicationBuilder> SeedAuthDataAsync(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Đảm bảo DB được migrate trước khi seed
            await context.Database.MigrateAsync().ConfigureAwait(false);

            if (!await context.Users.AnyAsync())
            {
                var usersToSeed = new List<User>
                {
                    new User
                    {
                        Id = new Guid("1a1a1a1a-1a1a-1a1a-1a1a-1a1a1a1a1a1a"),
                        Email = "thuanndmqe170240@fpt.edu.vn",
                        FirstName = "Thuan",
                        LastName = "Nguyen Dao Minh Thuan",
                        Phone = "0838683861",
                        Password = "$2a$11$nqgqbLHtqF6NRshEbN2VWeKvVz/M3FO2UwuMUYgEuSseyBLbZ8Nlm",
                        isConfirm = true,
                        Role = nameof(Role.Admin),
                    },

                    new User
                    {
                        Id = new Guid("2a2a2a2a-2a2a-2a2a-2a2a-2a2a2a2a2a2a"),
                        Email = "tuyenhttqe170226@fpt.edu.vn",
                        FirstName = "Tuyen",
                        LastName = "Huynh Thi Thanh",
                        Phone = "0838683868",
                        Password = "$2a$11$nqgqbLHtqF6NRshEbN2VWeKvVz/M3FO2UwuMUYgEuSseyBLbZ8Nlm",
                        isConfirm = true,
                        Role = nameof(Role.Admin),
                    },

                    new User
                    {
                        Id = new Guid("3a3a3a3a-3a3a-3a3a-3a3a-3a3a3a3a3a3a"),
                        Email = "hungntkqe170153@fpt.edu.vn",
                        FirstName = "Hung",
                        LastName = "Nguyen Tu Khanh",
                        Phone = "0838683866",
                        Password = "$2a$11$nqgqbLHtqF6NRshEbN2VWeKvVz/M3FO2UwuMUYgEuSseyBLbZ8Nlm",
                        isConfirm = true,
                        Role = nameof(Role.Admin),
                    },

                    new User
                    {
                        Id = new Guid("4a4a4a4a-4a4a-4a4a-4a4a-4a4a4a4a4a4a"),
                        Email = "thinhntgqe170209@fpt.edu.vn",
                        FirstName = "Thinh",
                        LastName = "Nguyen Truong Gia",
                        Phone = "0838683866",
                        Password = "$2a$11$nqgqbLHtqF6NRshEbN2VWeKvVz/M3FO2UwuMUYgEuSseyBLbZ8Nlm",
                        isConfirm = true,
                        Role = nameof(Role.Admin),
                    },

                    new User
                    {
                        Id = new Guid("5a5a5a5a-5a5a-5a5a-5a5a-5a5a5a5a5a5a"),
                        Email = "hoangngqe170225@fpt.edu.vn",
                        FirstName = "Hoang",
                        LastName = "Ngo Gia",
                        Phone = "0838683865",
                        Password = "$2a$11$nqgqbLHtqF6NRshEbN2VWeKvVz/M3FO2UwuMUYgEuSseyBLbZ8Nlm",
                        isConfirm = true,
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
            }
            return app;
        }
    }
}
