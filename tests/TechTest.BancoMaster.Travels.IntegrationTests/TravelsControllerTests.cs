
using Microsoft.AspNetCore.TestHost;
using System.Net;

namespace TechTest.BancoMaster.Travels.IntegrationTests
{
    public class TravelsControllerTests
    {
        private readonly HttpClient _client = Fixtures.BuildTestServer();

        [Fact]
        public async Task Requesting_Cheapest_Travel_Should_Find_Trip()
        {
            var startingPoint = "GRU";
            var destination = "CDG";

            var response = await _client
                .GetAsync($"/api/Travels/from/{startingPoint}/to/{destination}");

            var content = await response.Content.ReadAsStringAsync();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().Contain("\"TotalTripAmount\": 40.00");
        }

    }
}
