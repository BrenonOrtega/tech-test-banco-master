using Microsoft.Extensions.DependencyInjection;
using TechTest.BancoMaster.Travels.Domain.Travels;
using TechTest.BancoMaster.Travels.Domain.Travels.Repositories;
using TechTest.BancoMaster.Travels.Infra.Travels.Repositories;

namespace TechTest.BancoMaster.Travels.Infra.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddRepositories();

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ITravelRepository, TravelRepository>();

            // for simplicity of having a seeded 'database' :)
            services.AddSingleton(new Dictionary<string, Travel>()
            {
                { "GRU#BRC", new Travel(source: "GRU", destination: "BRC", amount: 1000) },
                { "BRC#SCL", new Travel(source: "BRC", destination: "SCL", amount: 500) },
                { "GRU#CDG", new Travel(source: "GRU", destination: "CDG", amount: 7500) },
                { "GRU#SCL", new Travel(source: "GRU", destination: "SCL", amount: 2000) },
                { "GRU#ORL", new Travel(source: "GRU", destination: "ORL", amount: 5600) },
                { "ORL#CDG", new Travel(source: "ORL", destination: "CDG", amount: 500) },
                { "SCL#ORL", new Travel(source: "SCL", destination: "ORL", amount: 2000) },
            });

            return services;
        }
    }
}
