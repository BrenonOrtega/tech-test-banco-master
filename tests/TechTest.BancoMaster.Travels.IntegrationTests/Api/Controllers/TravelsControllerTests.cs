
using Flurl.Http;
using System.Net;
using TechTest.BancoMaster.Travels.Api.Models.Requests;

namespace TechTest.BancoMaster.Travels.IntegrationTests.Api.Controllers
{
    public class TravelsControllerTests
    {
        private readonly HttpClient _client = Fixtures.BuildTestServer();
        private readonly IFlurlClient _flurlClient;

        public TravelsControllerTests()
        {
            _flurlClient = new FlurlClient(_client);
        }

        [Fact]
        public async Task Adding_Travel_Should_Find_Trip()
        {
            var request = new PostTravelRequest()
            {
                Amount = 3000,
                StartingPoint = "CRM",
                Destination = "ERP"

            };

            var response = await _flurlClient
                .Request("/api/Travels")
                .PostJsonAsync(request);

            var content = await response.ResponseMessage.Content.ReadAsStringAsync();

            response.StatusCode.Should().Be((int)HttpStatusCode.OK);
            content.Should().Contain("CRM#ERP");
        }

        [Fact]
        public async Task Requesting_Cheapest_Travel_Should_Find_Trip()
        {
            var startingPoint = "GRU";
            var destination = "CDG";

            var response = await _client
                .GetAsync($"/api/Travels/from/{startingPoint}/to/{destination}");

            var content = await response.Content.ReadAsStringAsync();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().Contain("GRU -> BRC -> SCL -> ORL -> CDG -> Costing a total of 40.00");
        }

        [Fact]
        public async Task Requesting_StartingPoint_Travel_Should_Find_Trips()
        {
            var startingPoint = "GRU";

            var response = await _client
                .GetAsync($"/api/Travels/startingPoint/{startingPoint}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Requesting_Inexistent_StartingPoint_Travel_Should_Be_NotFound()
        {
            var startingPoint = "INEXISTENT_LOCATION_TEST";

            var response = await _client
                .GetAsync($"/api/Travels/startingPoint/{startingPoint}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Requesting_Destination_Travel_Should_Find_Trips()
        {
            var destination = "CDG";

            var response = await _client
                .GetAsync($"/api/Travels/destination/{destination}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Requesting_Inexistent_Destination_Travel_Should_Be_NotFound()
        {
            var destination = "INEXISTENT_LOCATION_TEST";

            var response = await _client
                .GetAsync($"/api/Travels/destination/{destination}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
