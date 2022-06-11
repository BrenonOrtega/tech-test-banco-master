using Awarean.Sdk.Result;
using Microsoft.AspNetCore.Mvc;
using System.Net;
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
        public async Task<IActionResult> GetCheapestTravelAsync(string from, string to)
        {
            if (string.IsNullOrEmpty(from) || string.IsNullOrEmpty(to))
                return BadRequest(Error.Create("INVALID_PARAMETERS", "Starting point or destination point missing"));

            var result = await _service.GetCheapestPathAsync(from, to);

            if (result.IsFailed)
                return BadRequest(result.Error);

            return Ok(result.Value);
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCheapestTravelAsync([FromQuery] int offset, [FromQuery] int pageSize = 100)
        {
            var result = await _service.GetTravelsAsync(offset, pageSize);

            if (result.IsFailed)
                return BadRequest(result.Error);

            return Ok(result.Value);
        }
    }
}
