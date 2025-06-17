using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PhyGen.Application.Mapping;
using PhyGen.Domain.Interfaces;
using PhyGen.Infrastructure.Persistence.Repositories;
using PhyGen.Insfrastructure.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
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
        }
    }
}
