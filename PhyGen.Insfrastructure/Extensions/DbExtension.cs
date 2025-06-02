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

            if (!await context.Curriculums.AnyAsync())
            {
                var physicsCurriculums = new List<Curriculum>
                {
                    new Curriculum
                    {
                        Id = new Guid("a1b2c3d4-e5f6-7890-1234-56789abcdef0"),
                        Name = "Vật lý 10",
                        Grade = "10",
                        Description = "Khung chương trình Vật lý lớp 10."
                    },
                    new Curriculum
                    {
                        Id = new Guid("b2c3d4e5-f678-9012-3456-789abcdef012"),
                        Name = "Vật lý 11",
                        Grade = "11",
                        Description = "Khung chương trình Vật lý lớp 11."
                    },
                    new Curriculum
                    {
                        Id = new Guid("c3d4e5f6-7890-1234-5678-90abcdef1234"),
                        Name = "Vật lý 12",
                        Grade = "12",
                        Description = "Khung chương trình Vật lý lớp 12."
                    }
                };

                await context.Curriculums.AddRangeAsync(physicsCurriculums);
                await context.SaveChangesAsync();
            }

            if (!await context.BookSeries.AnyAsync())
            {
                var bookSeries = new List<BookSeries>
                {
                    new BookSeries
                    {
                        Id = new Guid("d4e5f678-9012-3456-7890-1234567890ab"),
                        Name = "Kết nối tri thức với cuộc sống",
                    },
                    new BookSeries
                    {
                        Id = new Guid("e5f67890-1234-5678-90ab-cdef12345678"),
                        Name = "Cánh Diều",
                    }
                };
                await context.BookSeries.AddRangeAsync(bookSeries).ConfigureAwait(false);
                await context.SaveChangesAsync().ConfigureAwait(false);
            }

            if (!await context.Books.AnyAsync())
            {
                var books = new List<Book>
                    {
                        new Book
                        {
                            Id = new Guid("a1111111-1111-1111-1111-111111111111"),
                            Name = "Vật lý 10 - Kết nối tri thức với cuộc sống",
                            SeriesId = new Guid("d4e5f678-9012-3456-7890-1234567890ab"),
                            Author = "Nhà xuất bản Giáo dục Việt Nam",
                            PublicationYear = 2018,
                        },
                        new Book
                        {
                            Id = new Guid("a2222222-2222-2222-2222-222222222222"),
                            Name = "Vật lý 11 - Kết nối tri thức với cuộc sống",
                            SeriesId = new Guid("d4e5f678-9012-3456-7890-1234567890ab"),
                            Author = "Nhà xuất bản Giáo dục Việt Nam",
                            PublicationYear = 2018,
                        },
                        new Book
                        {
                            Id = new Guid("a3333333-3333-3333-3333-333333333333"),
                            Name = "Vật lý 12 - Kết nối tri thức với cuộc sống",
                            SeriesId = new Guid("d4e5f678-9012-3456-7890-1234567890ab"),
                            Author = "Nhà xuất bản Giáo dục Việt Nam",
                            PublicationYear = 2018,
                        },

                        new Book
                        {
                            Id = new Guid("b1111111-1111-1111-1111-111111111111"),
                            Name = "Vật lý 10 - Cánh Diều",
                            SeriesId = new Guid("e5f67890-1234-5678-90ab-cdef12345678"),
                            Author = "Công ty Đầu tư Xuất bản - Thiết bị giáo dục Việt Nam (VEPIC) phối hợp với Nhà xuất bản Đại học Sư phạm Thành phố Hồ Chí Minh, Nhà xuất bản Đại học Huế và Nhà xuất bản Đại học Sư phạm",
                            PublicationYear = 2018,
                        },
                        new Book
                        {
                            Id = new Guid("b2222222-2222-2222-2222-222222222222"),
                            Name = "Vật lý 11 - Cánh Diều",
                            SeriesId = new Guid("e5f67890-1234-5678-90ab-cdef12345678"),
                            Author = "Công ty Đầu tư Xuất bản - Thiết bị giáo dục Việt Nam (VEPIC) phối hợp với Nhà xuất bản Đại học Sư phạm Thành phố Hồ Chí Minh, Nhà xuất bản Đại học Huế và Nhà xuất bản Đại học Sư phạm",
                            PublicationYear = 2018,
                        },
                        new Book
                        {
                            Id = new Guid("b3333333-3333-3333-3333-333333333333"),
                            Name = "Vật lý 12 - Cánh Diều",
                            SeriesId = new Guid("e5f67890-1234-5678-90ab-cdef12345678"),
                            Author = "Công ty Đầu tư Xuất bản - Thiết bị giáo dục Việt Nam (VEPIC) phối hợp với Nhà xuất bản Đại học Sư phạm Thành phố Hồ Chí Minh, Nhà xuất bản Đại học Huế và Nhà xuất bản Đại học Sư phạm",
                            PublicationYear = 2018,
                        }
                    };

                await context.Books.AddRangeAsync(books).ConfigureAwait(false);
                await context.SaveChangesAsync().ConfigureAwait(false);
            }

            return app;
        }
    }
}
