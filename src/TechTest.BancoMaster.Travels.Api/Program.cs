var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

var startup = new Startup(config);

startup.ConfigureServices(builder.Services);

var app = builder.Build();

startup.Configure(app);

app.Run();

public partial class Program
{

}