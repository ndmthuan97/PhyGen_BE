using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PhyGen.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Insfrastructure.Persistence.DbContexts
{
    public class AppDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Declare DbSets corresponding to tables in the database
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

        // Configure entities and table mappings, set constraints and properties for columns
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.UserName).IsUnique();  
            });

            modelBuilder.Entity<Transaction>(e => e.Property(p => p.Amount).HasPrecision(18, 2));

            modelBuilder.Entity<Curriculum>(e =>
            {
                e.Property(p => p.Name).HasMaxLength(255).IsRequired();
                e.Property(p => p.Grade).HasMaxLength(50);
            });

            modelBuilder.Entity<Chapter>(e => e.Property(p => p.Title).HasMaxLength(255).IsRequired());

            modelBuilder.Entity<Question>(e =>
            {
                e.Property(p => p.Type).HasMaxLength(50);
                e.Property(p => p.Level).HasMaxLength(50);
                e.Property(p => p.Image).HasMaxLength(500);
            });

            modelBuilder.Entity<Answer>(e => e.Property(p => p.IsCorrect).HasDefaultValue(false));

            modelBuilder.Entity<Matrix>(e =>
            {
                e.Property(p => p.Name).HasMaxLength(255);
                e.Property(p => p.Grade).HasMaxLength(50);
            });

            modelBuilder.Entity<MatrixDetail>(e =>
            {
                e.Property(p => p.Level).HasMaxLength(50);
                e.Property(p => p.Quantity).HasDefaultValue(0);
            });

            modelBuilder.Entity<Exam>(e =>
            {
                e.Property(p => p.Title).HasMaxLength(255);
                e.HasOne(p => p.Creator)
                 .WithMany()
                 .HasForeignKey(p => p.CreatedBy)
                 .OnDelete(DeleteBehavior.Restrict); // quan trọng!
            });

            modelBuilder.Entity<ExamPaper>(e => e.Property(p => p.Version).HasMaxLength(50));

            modelBuilder.Entity<ExamCategory>(e => e.Property(p => p.Name).HasMaxLength(255));

            modelBuilder.Entity<BookSeries>(e => e.Property(p => p.Name).HasMaxLength(255));

            modelBuilder.Entity<Book>(e =>
            {
                e.Property(p => p.Title).HasMaxLength(255);
                e.Property(p => p.Author).HasMaxLength(255);
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
            modelBuilder.Entity<User>().HasQueryFilter(e => e.DeletedAt == null);
            modelBuilder.Entity<Curriculum>().HasQueryFilter(e => e.DeletedAt == null);
            modelBuilder.Entity<Chapter>().HasQueryFilter(e => e.DeletedAt == null);
            modelBuilder.Entity<ChapterUnit>().HasQueryFilter(e => e.DeletedAt == null);
            modelBuilder.Entity<Matrix>().HasQueryFilter(e => e.DeletedAt == null);
            modelBuilder.Entity<MatrixDetail>().HasQueryFilter(e => e.DeletedAt == null);
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
