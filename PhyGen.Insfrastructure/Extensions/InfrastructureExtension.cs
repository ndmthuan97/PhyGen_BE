using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PhyGen.Application.Authentication.Interface;
using PhyGen.Application.Curriculums.Queries;
using PhyGen.Domain.Interfaces.Repositories;
using PhyGen.Insfrastructure.Identity;
using PhyGen.Insfrastructure.Persistence.Repositories;
using PhyGen.Insfrastructure.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Insfrastructure.Extensions
{
    // Extension methods for setting up infrastructure-related services, including authentication, identity, repository injection...
    public static class InfrastructureExtension
    {
        public static IServiceCollection AddCoreInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var applicationAssemblies = new[] {
                typeof(GetAllCurriculumsQuery).Assembly,
                //typeof(ModelMappingProfile).Assembly
            };

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(applicationAssemblies));
            services.AddAutoMapper(applicationAssemblies);

            
            CommonInfrastrucutre(services, configuration);
            return services;
        }

        private static void CommonInfrastrucutre(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(IAsyncRepository<,>), typeof(RepositoryBase<,>));

            services.AddScoped<IAuthService, AuthService>();

            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

            services.AddScoped<ICurriculumRepository, CurriculumRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            
            services.AddScoped<IChapterRepository, ChapterRepository>();
            services.AddScoped<IChapterUnitRepository, ChapterUnitRepository>();

            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<IBookSeriesRepository, BookSeriesRepository>();
            services.AddScoped<IBookDetailRepository, BookDetailRepository>();

            services.AddScoped<IQuestionRepository, QuestionRepository>();
            services.AddScoped<IAnswerRepository, AnswerRepository>();
            services.AddScoped<IExamRepository, ExamRepository>();

            services.AddScoped<IMatrixRepository, MatrixRepository>();
            services.AddScoped<IMatrixDetailRepository, MatrixDetailRepository>();
        }
    }
}
