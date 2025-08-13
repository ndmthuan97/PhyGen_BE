using MediatR;
using Microsoft.EntityFrameworkCore;
using Net.payOS.Types;
using PhyGen.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Infrastructure.Persistence.DbContexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Declare DbSets corresponding to tables in the database
        public DbSet<Chapter> Chapters { get; set; }
        public DbSet<ContentFlow> ContentFlows { get; set; }
        public DbSet<ContentItem> ContentItems { get; set; }
        public DbSet<Curriculum> Curriculums { get; set; }
        public DbSet<EmailOtpManager> EmailOtpManagers { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<ExamCategory> ExamCategories { get; set; }
        public DbSet<ExamVersion> ExamVersions { get; set; }
        public DbSet<Matrix> Matrices { get; set; }
        public DbSet<MatrixSection> MatrixSections { get; set; }
        public DbSet<MatrixSectionDetail> MatrixSectionDetails { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionMedia> QuestionMedias { get; set; }
        public DbSet<QuestionSection> QuestionSections { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<SubjectBook> SubjectBooks { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Domain.Entities.Transaction> Transactions { get; set; } 


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
                e.Property(p => p.SubjectBookId).IsRequired();
            });
            modelBuilder.Entity<ContentFlow>(e =>
            {
                e.Property(p => p.CurriculumId).IsRequired();
                e.Property(p => p.SubjectId).IsRequired();
                e.Property(p => p.Name).IsRequired();
            });

            modelBuilder.Entity<ContentItem>(e =>
            {
                e.Property(p => p.ContentFlowId).IsRequired();
                e.Property(p => p.Name).IsRequired();
            });

            modelBuilder.Entity<Curriculum>(e =>
            {
                e.Property(p => p.Name).HasMaxLength(255).IsRequired();
            });

            modelBuilder.Entity<EmailOtpManager>(e =>
            {
                e.Property(p => p.Email).HasMaxLength(256);
                e.Property(p => p.Otptext).IsRequired();
                e.Property(p => p.Otptype).HasMaxLength(50);
                e.Property(p => p.Expiration).IsRequired();
            });
            modelBuilder.Entity<Exam>(e =>
            {
                e.Property(p => p.UserId).IsRequired();
                e.Property(p => p.ExamCategoryId).IsRequired();
                e.Property(p => p.Title).IsRequired();
                e.Property(p => p.Description).HasColumnType("text");
                e.Property(p => p.Grade).IsRequired();
                e.Property(p => p.VersionCount).IsRequired();
                e.Property(p => p.RandomizeQuestions).IsRequired();

            });

            modelBuilder.Entity<ExamCategory>(e =>
            {
                e.Property(p => p.Name).IsRequired();
            });

            modelBuilder.Entity<ExamVersion>(e =>
            {
                e.Property(p => p.ExamId).IsRequired();
                e.Property(p => p.Code).IsRequired();
            });

            modelBuilder.Entity<Matrix>(e =>
            {
                e.Property(p => p.SubjectId).IsRequired();
                e.Property(p => p.ExamCategoryId).IsRequired();
            });

            modelBuilder.Entity<MatrixSection>(e =>
            {
                e.Property(p => p.MatrixId).IsRequired();
                e.Property(p => p.Title).IsRequired();
                e.Property(p => p.Description).HasColumnType("text");
            });

            modelBuilder.Entity<MatrixSectionDetail>(e =>
            {
                e.Property(p => p.MatrixSectionId).IsRequired();
                e.Property(p => p.SectionId).IsRequired();
                e.Property(p => p.ContentItemId).IsRequired();
                e.Property(p => p.Title).IsRequired();
                e.Property(p => p.Description).HasColumnType("text");
            });

            modelBuilder.Entity<Notification>(e => e.Property(p => p.IsRead).HasDefaultValue(false));
            modelBuilder.Entity<Payment>(e =>
            {
                e.Property(p => p.UserId).IsRequired();
            });

            modelBuilder.Entity<Question>(e =>
            {
                e.Property(p => p.TopicId);
                e.Property(p => p.Content).IsRequired();
            });

            modelBuilder.Entity<QuestionMedia>(e =>
            {
                e.Property(p => p.QuestionId).IsRequired();
                e.Property(p => p.MediaType).IsRequired();
                e.Property(p => p.Url).IsRequired();
            });

            modelBuilder.Entity<QuestionSection>(e =>
            {
                e.Property(p => p.QuestionId).IsRequired();
                e.Property(p => p.SectionId).IsRequired();
            });

            modelBuilder.Entity<Section>(e =>
            {
                e.Property(p => p.ExamId).IsRequired();
                e.Property(p => p.Title).IsRequired();
                e.Property(p => p.Description).HasColumnType("text");
            });

            modelBuilder.Entity<Subject>(e =>
            {
                e.Property(p => p.Name).IsRequired();
            });

            modelBuilder.Entity<SubjectBook>(e =>
            {
                e.Property(p => p.SubjectId).IsRequired();
                e.Property(p => p.Name).IsRequired();
                e.Property(p => p.Grade).IsRequired();
            });

            modelBuilder.Entity<Topic>(e =>
            {
                e.Property(p => p.ChapterId).IsRequired();
                e.Property(p => p.Name).IsRequired();
            });

            modelBuilder.Entity<Domain.Entities.Transaction>(e =>
            {
                e.Property(p => p.UserId).IsRequired();
            });

            modelBuilder.Entity<User>(e =>
            {
                e.Property(p => p.FirstName).HasMaxLength(50).IsRequired();
                e.Property(p => p.LastName).HasMaxLength(50).IsRequired();
                e.Property(p => p.Email).IsRequired();
                e.Property(p => p.Password).IsRequired();
                e.Property(p => p.isConfirm).IsRequired();
            });

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
