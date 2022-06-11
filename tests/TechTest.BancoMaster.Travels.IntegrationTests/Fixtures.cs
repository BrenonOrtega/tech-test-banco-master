using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using TechTest.BancoMaster.Travels.Domain.Travels;

namespace TechTest.BancoMaster.Travels.IntegrationTests;

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

    public static List<Travel> GetTravelList() => new List<Travel>
    {
        new Travel(source: "GRU", destination: "BRC", amount: 1000),
        new Travel(source: "BRC", destination: "SCL", amount: 500),
        new Travel(source: "GRU", destination: "CDG", amount: 7500),
        new Travel(source: "GRU", destination: "SCL", amount: 2000),
        new Travel(source: "GRU", destination: "ORL", amount: 5600),
        new Travel(source: "ORL", destination: "CDG", amount: 500),
        new Travel(source: "SCL", destination: "ORL", amount: 2000),
    };
}
