using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PhyGen.Application.Curriculums.Queries;
using PhyGen.Domain.Interfaces.Repositories;
using PhyGen.Insfrastructure.Persistence.Repositories;
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
                typeof(GetAllCurriculumsQuery).Assembly
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
        }
    }
}
