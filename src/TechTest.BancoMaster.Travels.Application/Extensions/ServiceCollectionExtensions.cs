using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TechTest.BancoMaster.Travels.Application.CheapestRouteCalculation;
using TechTest.BancoMaster.Travels.Application.Travels.Services;
using TechTest.BancoMaster.Travels.Domain.CheapestRouteCalculation;
using TechTest.BancoMaster.Travels.Domain.Structures;
using TechTest.BancoMaster.Travels.Domain.Travels;

namespace TechTest.BancoMaster.Travels.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<ITravelService, TravelService>();
            services.AddScoped<ICheapestTravelFinder, CheapestTravelFinder>();

            services.AddBuilders();
            services.AddGraphEngines();

            return services;
        }

        public static IServiceCollection AddBuilders(this IServiceCollection services)
        {
            services.AddScoped<INodeBuilder, NodeBuilder>();
            services.AddScoped<IGraphBuilder, GraphBuilder>();

            return services;
        }

        public static IServiceCollection AddGraphEngines(this IServiceCollection services)
        {
            services.AddScoped<ITravelGraphBuildEngine, TravelGraphBuildEngine>();
            services.AddScoped<Action<string, object[]>>(x => x.GetRequiredService<ILogger>().LogInformation);
            return services;
        }
    }
}
