using Microsoft.AspNetCore.HttpLogging;
using TechTest.BancoMaster.Travels.Application.Extensions;
using TechTest.BancoMaster.Travels.Infra.Extensions;

public class Startup 
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        services.AddLogging()
            .AddHttpLogging(x => x.LoggingFields = HttpLoggingFields.Request | HttpLoggingFields.ResponseStatusCode | HttpLoggingFields.Response);

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddApplicationServices()
            .AddInfrastructure();
    }
     
    public void Configure(WebApplication app)
    {
        var env = app.Environment;

        if (env.IsDevelopment() || env.IsEnvironment("Docker"))
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = string.Empty;
            });
        }
        app.UseStaticFiles();
        app.UseRouting();
        app.UseEndpoints(x => x.MapControllers());
    }
}

