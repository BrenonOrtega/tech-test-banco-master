using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TechTest.BancoMaster.Travels.IntegrationTests
{
    public static class Fixtures
    {
        public static HttpClient BuildTestServer()
        {
            var client = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(x => x.UseTestServer())
                .CreateClient()
                ;

            return client;
        }
    }
}
