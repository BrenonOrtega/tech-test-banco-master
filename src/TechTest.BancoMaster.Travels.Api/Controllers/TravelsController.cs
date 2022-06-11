using Awarean.Sdk.Result;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using TechTest.BancoMaster.Travels.Api.Models.Requests;
using TechTest.BancoMaster.Travels.Application;
using TechTest.BancoMaster.Travels.Domain.Travels;
using TechTest.BancoMaster.Travels.Domain.Travels.Contracts;

namespace TechTest.BancoMaster.Travels.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TravelsController : ControllerBase
{
    private readonly ITravelService _service;

    public TravelsController(ITravelService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    [HttpGet("cheapest/from/{from}/to/{to}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetCheapestTravelAsync(string from, string to)
    {
        if (string.IsNullOrEmpty(from) || string.IsNullOrEmpty(to))
            return BadRequest(Error.Create("INVALID_PARAMETERS", "Starting point or destination point missing"));

        var result = await _service.GetCheapestPathAsync(from, to);

        if (result.IsFailed)
            return BadRequest(result.Error);

        return BuildCheapestPathResponse(result.Value);
    }

    private OkObjectResult BuildCheapestPathResponse(ICheapestTravelResponse value) =>
        Ok(new { value.StartingPoint, value.Destination, value.TotalAmount, Response = value.DescribeCheapestTravel() });

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetTravelsAsync([FromQuery] int offset, [FromQuery] int pageSize = 100)
    {
        var result = await _service.GetTravelsAsync(offset, pageSize);

        if (result.IsFailed)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }


    [HttpGet("startingPoint/{startingPoint}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetStartingPointAsync([FromRoute] string startingPoint)
    {
        var result = await _service.GetByStartingPointAsync(startingPoint);
        return HandleResult(result);
    }

    [HttpGet("destination/{destination}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetDestinationAsync([FromRoute] string destination)
    {
        var result = await _service.GetByDestinationAsync(destination);
        return HandleResult(result);
    }

    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Post([FromBody] PostTravelRequest request)
    {
        var result = await _service.AddTravelAsync(request.StartingPoint, request.Destination, request.Amount);

        if (result.IsFailed)
        {
            if(result.Error == ApplicationErrors.FailedAddingTravel)
                return Problem();

            return BadRequest(result.Error);
        }

        return Ok(new { Id = result.Value });
    }

    [HttpPatch]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> UpdateTravelValueAsync([FromBody] UpdateTravelValueRequest request)
    {
        var result = await _service.UpdateTravelAsync(request.StartingPoint, request.Destination, request.Amount);

        if (result.IsFailed && result.Error == ApplicationErrors.CannotChangeTravelsName)
            return BadRequest(result.Error);

        if (result.IsFailed && result.Error == ApplicationErrors.TravelNotFound)
            return NotFound(result.Error);

        if (result.IsFailed)
            return Problem(JsonSerializer.Serialize(result.Error));

        return Ok();
    }

    [HttpDelete("{travelId}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> DeleteTravelValueAsync(string travelId)
    {
        var result = await _service.DeleteTravelAsync(travelId);

        if (result.IsFailed && result.Error == ApplicationErrors.FailedDeletingTravel)
            return BadRequest(result.Error);

        if (result.IsFailed)
            return Problem(JsonSerializer.Serialize(result.Error));

        return Ok();
    }


    private IActionResult HandleResult(Result<IEnumerable<Travel>> result)
    {
        if (result.IsFailed)
        {
            if (result.Error == ApplicationErrors.TravelNotFound)
                return NotFound();

            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }
}