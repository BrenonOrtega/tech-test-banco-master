using Awarean.Sdk.Result;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TechTest.BancoMaster.Travels.Api.Models.Requests;
using TechTest.BancoMaster.Travels.Domain.Travels;

namespace TechTest.BancoMaster.Travels.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TravelsController : ControllerBase
    {
        private readonly ITravelService _service;

        public TravelsController(ITravelService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [HttpGet("from/{from}/to/{to}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetCheapestTravelAsync([FromRoute] SearchCheapestTravelRequest request)
        {
            if (string.IsNullOrEmpty(request.From) || string.IsNullOrEmpty(request.To))
                return BadRequest(Error.Create("INVALID_PARAMETERS", "Starting point or destination point missing"));

            var result = await _service.GetCheapestPathAsync(request.From, request.To);

            if (result.IsFailed)
                return BadRequest(result.Error);

            return Ok(result.Value);
        }
    }
}
