using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PhyGen.Application.Admin.Interfaces;
using PhyGen.Application.Authentication.DTOs.Dtos;
using PhyGen.Application.Authentication.Interface;
using PhyGen.Application.Authentication.Models.Requests;
using PhyGen.Application.Mapping;
using PhyGen.Application.PayOs.Interfaces;
using PhyGen.Domain.Interfaces;
using PhyGen.Infrastructure.Persistence.Repositories;
using PhyGen.Infrastructure.Service;
using PhyGen.Insfrastructure.BackgroundServices;
using PhyGen.Insfrastructure.Persistence.Repositories;
using PhyGen.Insfrastructure.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Insfrastructure.Extensions
{
    public static class InfrastructureExtension
    {
        public static IServiceCollection AddCoreInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var applicationAssemblies = new[] {
                typeof(CoreMappingProfile).Assembly,
            };

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(applicationAssemblies));
            services.AddAutoMapper(applicationAssemblies);


            CommonInfrastrucutre(services, configuration);
            return services;
        }

        private static void CommonInfrastrucutre(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(IAsyncRepository<,>), typeof(RepositoryBase<,>));
            services.AddScoped<ICurriculumRepository, CurriculumRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ISubjectRepository, SubjectRepository>();
            services.AddScoped<ISubjectBookRepository, SubjectBookRepository>();
            services.AddScoped<IChapterRepository, ChapterRepository>();
            services.AddScoped<ITopicRepository, TopicRepository>();
            services.AddScoped<IContentFlowRepository, ContentFlowRepository>();
            services.AddScoped<IContentItemRepository, ContentItemRepository>();
            services.AddScoped<IExamCategoryRepository, ExamCategoryRepository>();
            services.AddScoped<IContentItemExamCategoryRepository, ContentItemExamCategoryRepository>();
            services.AddScoped<IExamCategoryChapterRepository, ExamCategoryChapterRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<IStatisticService, AdminService>();
            services.AddHostedService<ExpirePaymentBackgroundService>();
            services.AddScoped<IQuestionRepository, QuestionRepository>();
            services.AddScoped<IMatrixRepository, MatrixRepository>();
            services.AddScoped<IMatrixSectionRepository, MatrixSectionRepository>();
            services.AddScoped<IMatrixSectionDetailRepository, MatrixSectionDetailRepository>();
            services.AddScoped<IMatrixContentItemRepository, MatrixContentItemRepository>();
            services.AddScoped<IQuestionMediaRepository, QuestionMediaRepository>();
            services.AddScoped<ISectionRepository, SectionRepository>();
            services.AddScoped<IQuestionSectionRepository, QuestionSectionRepository>();
        }
    }
}
