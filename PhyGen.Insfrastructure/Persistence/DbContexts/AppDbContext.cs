using MediatR;
using Microsoft.EntityFrameworkCore;
using PhyGen.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Insfrastructure.Persistence.DbContexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Declare DbSets corresponding to tables in the database
        public DbSet<Chapter> Chapters { get; set; }
        public DbSet<ChapterUnit> ChapterUnits { get; set; }
        public DbSet<ContentFlow> ContentFlows { get; set; }
        public DbSet<ContentItem> ContentItems { get; set; }
        public DbSet<ContentItemExamCategory> ContentItemExamCategories { get; set; }
        public DbSet<Curriculum> Curriculums { get; set; }
        public DbSet<EmailOtpManager> EmailOtpManagers { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<ExamCategory> ExamCategories { get; set; }
        public DbSet<ExamCategoryChapter> ExamCategoryChapters { get; set; }
        public DbSet<ExamQuestion> ExamQuestions { get; set; }
        public DbSet<Matrix> Matrices { get; set; }
        public DbSet<MatrixContentItem> MatrixContentItems { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionMedia> QuestionMedias { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<SubjectCurriculum> SubjectCurriculums { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<User> Users { get; set; }



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

            modelBuilder.Entity<Chapter>(e =>
            {
                e.Property(p => p.Name).HasMaxLength(255).IsRequired();
                e.Property(p => p.SubjectCurriculumId).IsRequired();
                e.Property(p => p.OrderNo);
                e.Property(p => p.CreatedBy);
                e.Property(p => p.CreatedAt).IsRequired();
                e.Property(p => p.UpdatedBy);
                e.Property(p => p.UpdatedAt);
                e.Property(p => p.DeletedBy);
                e.Property(p => p.DeletedAt);
            });

            modelBuilder.Entity<ChapterUnit>(e =>
            {
                e.Property(p => p.ChapterId).IsRequired();
                e.Property(p => p.Name).HasMaxLength(255).IsRequired();
                e.Property(p => p.Description).HasColumnType("text");
                e.Property(p => p.OrderNo);
                e.Property(p => p.CreatedBy);
                e.Property(p => p.CreatedAt).IsRequired();
                e.Property(p => p.UpdatedBy);
                e.Property(p => p.UpdatedAt);
                e.Property(p => p.DeletedBy);
                e.Property(p => p.DeletedAt);
            });

            modelBuilder.Entity<ContentFlow>(e =>
            {
                e.Property(p => p.Name).HasMaxLength(255).IsRequired();
                e.Property(p => p.Description).HasMaxLength(500).IsRequired();
                e.Property(p => p.SubjectId).IsRequired();
            });

            modelBuilder.Entity<ContentItem>(e =>
            {
                e.Property(p => p.ContentFlowId).IsRequired();
                e.Property(p => p.Title).HasMaxLength(255).IsRequired();
                e.Property(p => p.LearningOutcome).HasMaxLength(500).IsRequired();
                e.Property(p => p.CreatedAt).IsRequired();
            });

            modelBuilder.Entity<ContentItemExamCategory>(e =>
            {
                e.Property(p => p.ContentItemId).IsRequired();
                e.Property(p => p.ExamCategoryId).IsRequired();
            });

            modelBuilder.Entity<Curriculum>(e =>
            {
                e.Property(p => p.Name).HasMaxLength(255).IsRequired();
                e.Property(p => p.Grade);
            });

            modelBuilder.Entity<EmailOtpManager>(e =>
            {
                e.Property(p => p.Email).HasMaxLength(256);
                e.Property(p => p.Otptext).IsRequired();
                e.Property(p => p.Otptype).HasMaxLength(50);
                e.Property(p => p.Expiration).IsRequired();
                e.Property(p => p.Createddate);
            });

            modelBuilder.Entity<Exam>(e =>
            {
                e.Property(p => p.Title).HasMaxLength(255).IsRequired();
                e.Property(p => p.MatrixId).IsRequired();
                e.Property(p => p.CategoryId).IsRequired();
                e.Property(p => p.SubjectCurriculumId).IsRequired();
                e.Property(p => p.CreatedBy);
                e.Property(p => p.CreatedAt).IsRequired();
                e.Property(p => p.UpdatedBy);
                e.Property(p => p.UpdatedAt);
                e.Property(p => p.DeletedBy);
                e.Property(p => p.DeletedAt);
            });

            modelBuilder.Entity<ExamCategory>(e =>
            {
                e.Property(p => p.Name).HasMaxLength(255).IsRequired();
            });

            modelBuilder.Entity<ExamCategoryChapter>(e =>
            {
                e.Property(p => p.ExamCategoryId).IsRequired();
                e.Property(p => p.ChapterId).IsRequired();
            });

            modelBuilder.Entity<ExamQuestion>(e =>
            {
                e.Property(p => p.ExamId).IsRequired();
                e.Property(p => p.QuestionId).IsRequired();
            });

            modelBuilder.Entity<Matrix>(e =>
            {
                e.Property(p => p.Name).HasMaxLength(255).IsRequired();
                e.Property(p => p.Description).HasMaxLength(500).IsRequired();
                e.Property(p => p.Grade).HasMaxLength(50).IsRequired();
                e.Property(p => p.SubjectId).IsRequired();
                e.Property(p => p.ExamCategoryId).IsRequired();
                e.Property(p => p.CreatedBy);
                e.Property(p => p.CreatedAt).IsRequired();
                e.Property(p => p.UpdatedBy);
                e.Property(p => p.UpdatedAt);
                e.Property(p => p.DeletedBy);
                e.Property(p => p.DeletedAt);
            });

            modelBuilder.Entity<MatrixContentItem>(e =>
            {
                e.Property(p => p.MatrixId).IsRequired();
                e.Property(p => p.ContentItemId).IsRequired();
            });

            modelBuilder.Entity<Notification>(e => e.Property(p => p.IsRead).HasDefaultValue(false));

            modelBuilder.Entity<Question>(e =>
            {
                e.Property(p => p.Content).IsRequired();
                e.Property(p => p.Type);
                e.Property(p => p.Level);
                e.Property(p => p.Image);
                e.Property(p => p.ChapterUnitId).IsRequired();
                e.Property(p => p.Answer1);
                e.Property(p => p.Answer2);
                e.Property(p => p.Answer3);
                e.Property(p => p.Answer4);
                e.Property(p => p.Answer5);
                e.Property(p => p.Answer6);
                e.Property(p => p.CorrectAnswer);
                e.Property(p => p.CreatedBy);
                e.Property(p => p.CreatedAt).IsRequired();
                e.Property(p => p.UpdatedBy);
                e.Property(p => p.UpdatedAt);
                e.Property(p => p.DeletedBy);
                e.Property(p => p.DeletedAt);
            });

            modelBuilder.Entity<QuestionMedia>(e =>
            {
                e.Property(p => p.QuestionId).IsRequired();
                e.Property(p => p.MediaType);
                e.Property(p => p.Url).IsRequired();
            });

            modelBuilder.Entity<Subject>(e =>
            {
                e.Property(p => p.Name).IsRequired();
            });

            modelBuilder.Entity<SubjectCurriculum>(e =>
            {
                e.Property(p => p.SubjectId).IsRequired();
                e.Property(p => p.CurriculumId).IsRequired();
            });

            modelBuilder.Entity<Transaction>(e =>
            {
                e.Property(p => p.UserId).IsRequired();
                e.Property(p => p.Amount).HasColumnType("decimal(18,2)");
                e.Property(p => p.Description);
                e.Property(p => p.Status);
                e.Property(p => p.CreatedAt).IsRequired();
            });

            modelBuilder.Entity<User>(e =>
            {
                e.Property(p => p.FirstName).HasMaxLength(50).IsRequired();
                e.Property(p => p.LastName).HasMaxLength(50).IsRequired();
                e.Property(p => p.Email).IsRequired();
                e.Property(p => p.Password).IsRequired();
                e.Property(p => p.Gender);
                e.Property(p => p.photoURL);
                e.Property(p => p.Role);
                e.Property(p => p.Address);
                e.Property(p => p.Phone);
                e.Property(p => p.DateOfBirth);
                e.Property(p => p.isConfirm).IsRequired();
                e.Property(p => p.IsActive);
                e.Property(p => p.Coin);
                e.Property(p => p.CreatedAt);
            });

            // Gọi cấu hình delete và soft delete
            ConfigureCascadeDelete(modelBuilder);
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
    }
}
