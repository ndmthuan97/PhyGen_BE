using Microsoft.EntityFrameworkCore;
using PhyGen.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Insfrastructure.Persistence.DbContexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Declare DbSets corresponding to tables in the database
        public DbSet<User> Users { get; set; }
        public DbSet<Curriculum> Curriculums { get; set; }
        public DbSet<Chapter> Chapters { get; set; }
        public DbSet<ChapterUnit> ChapterUnits { get; set; }
        public DbSet<Matrix> Matrices { get; set; }
        public DbSet<MatrixDetail> MatrixDetails { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<ExamCategory> ExamCategories { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<ExamPaper> ExamPapers { get; set; }
        public DbSet<ExamPaperQuestion> ExamPaperQuestions { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<BookSeries> BookSeries { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookDetail> BookDetails { get; set; }
        public DbSet<EmailOtpManager> EmailOtpManager { get; set; }

        // Configure entities and table mappings, set constraints and properties for columns
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(string))
                    {
                        property.SetIsUnicode(true);
                    }
                }
            }

            modelBuilder.Entity<User>(e =>
            {
                e.Property(p => p.FirstName).HasMaxLength(50).IsRequired();
                e.Property(p => p.LastName).HasMaxLength(50).IsRequired();
                e.Property(p => p.Email).IsRequired();
                e.Property(p => p.Password).IsRequired();
                e.Property(p => p.photoURL);
                e.Property(p => p.Role);
                e.Property(p => p.Address);
                e.Property(p => p.Phone);
                e.Property(p => p.isConfirm).IsRequired();
            });

            modelBuilder.Entity<EmailOtpManager>(e =>
            {
                e.Property(p => p.Email).HasMaxLength(256);
                e.Property(p => p.Otptext).IsRequired();
                e.Property(p => p.Otptype).HasMaxLength(50);
                e.Property(p => p.Expiration).IsRequired();
                e.Property(p => p.Createddate);
            });


            modelBuilder.Entity<Transaction>(e => e.Property(p => p.Amount).HasPrecision(18, 2));

            modelBuilder.Entity<Curriculum>(e =>
            {
                e.Property(p => p.Name).HasMaxLength(255).IsRequired();
                e.Property(p => p.Grade).HasMaxLength(50);
                e.Property(p => p.Description).HasColumnType("nvarchar(max)");
            });

            modelBuilder.Entity<Chapter>(e =>
            {
                e.Property(p => p.Name).HasMaxLength(255).IsRequired();
                e.Property(p => p.CurriculumId);
                e.Property(p => p.OrderNo);

                e.HasOne(p => p.Curriculum)
                    .WithMany()
                    .HasForeignKey(p => p.CurriculumId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<ChapterUnit>(e => e.Property(p => p.Description).HasColumnType("nvarchar(max)"));

            modelBuilder.Entity<Question>(e =>
            {
                e.Property(p => p.Content).HasColumnType("nvarchar(max)").IsRequired();
                e.Property(p => p.Type).HasMaxLength(50);
                e.Property(p => p.Level).HasMaxLength(50);
                e.Property(p => p.Image).HasMaxLength(500);
            });


            modelBuilder.Entity<Answer>(e =>
            {
                e.Property(p => p.Content).HasColumnType("nvarchar(max)").IsRequired();
                e.Property(p => p.QuestionId).IsRequired();
            });


            modelBuilder.Entity<Matrix>(e =>
            {
                e.Property(p => p.Name).IsRequired();
                e.Property(p => p.Description).HasColumnType("nvarchar(max)");
                e.Property(p => p.Grade);
                e.Property(p => p.UserId).IsRequired();
            });

            modelBuilder.Entity<MatrixDetail>(e =>
            {
                e.Property(p => p.MatrixId).IsRequired();
                e.Property(p => p.ChapterId).IsRequired();
                e.Property(p => p.Level);
                e.Property(p => p.Quantity);
            });

            modelBuilder.Entity<Exam>(e =>
            {
                e.Property(p => p.Title).IsRequired();
                e.Property(p => p.MatrixId).IsRequired();
                e.Property(p => p.CategoryId).IsRequired();
                e.Property(p => p.CreatedBy).IsRequired();
            });

            modelBuilder.Entity<ExamPaper>(e => e.Property(p => p.Version).HasMaxLength(50));

            modelBuilder.Entity<ExamCategory>(e => e.Property(p => p.Name).HasMaxLength(255));

            modelBuilder.Entity<BookSeries>(e => e.Property(p => p.Name).HasMaxLength(255));

            modelBuilder.Entity<Book>(e =>
            {
                e.Property(p => p.Name).HasMaxLength(255);
                e.Property(p => p.Author).HasMaxLength(255);
            });

            modelBuilder.Entity<BookDetail>(e =>
            {
                e.HasKey(e => new { e.BookId, e.ChapterId }); // Composite Key

                e.Property(p => p.OrderNo);

                e.HasOne(p => p.Book)
                    .WithMany(b => b.BookDetails)
                    .HasForeignKey(p => p.BookId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(p => p.Chapter)
                    .WithMany(c => c.BookDetails)
                    .HasForeignKey(p => p.ChapterId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Notification>(e => e.Property(p => p.IsRead).HasDefaultValue(false));

            // Gọi cấu hình delete và soft delete
            ConfigureCascadeDelete(modelBuilder);
            ConfigureSoftDeleteFilter(modelBuilder);
        }

        // Configure all relationships in the model to not cascade deletion (restricted deletion)
        private void ConfigureCascadeDelete(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                         .SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

        // Set soft delete filter for entities with DeletedAt attribute
        // Records with DeletedAt != null will be automatically hidden from the default query
        private void ConfigureSoftDeleteFilter(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Curriculum>().HasQueryFilter(e => e.DeletedAt == null);
            modelBuilder.Entity<Chapter>().HasQueryFilter(e => e.DeletedAt == null);
            modelBuilder.Entity<ChapterUnit>().HasQueryFilter(e => e.DeletedAt == null);
            modelBuilder.Entity<Matrix>().HasQueryFilter(e => e.DeletedAt == null);
            modelBuilder.Entity<Question>().HasQueryFilter(e => e.DeletedAt == null);
            modelBuilder.Entity<Answer>().HasQueryFilter(e => e.DeletedAt == null);
            modelBuilder.Entity<Exam>().HasQueryFilter(e => e.DeletedAt == null);
            modelBuilder.Entity<ExamPaper>().HasQueryFilter(e => e.DeletedAt == null);
            modelBuilder.Entity<ExamPaperQuestion>().HasQueryFilter(e => e.DeletedAt == null);
            modelBuilder.Entity<Notification>().HasQueryFilter(e => e.DeletedAt == null);
            modelBuilder.Entity<BookSeries>().HasQueryFilter(e => e.DeletedAt == null);
            modelBuilder.Entity<Book>().HasQueryFilter(e => e.DeletedAt == null);
        }
    }
}
