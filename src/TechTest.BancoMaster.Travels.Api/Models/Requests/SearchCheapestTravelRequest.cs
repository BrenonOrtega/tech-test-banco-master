using Microsoft.AspNetCore.Mvc;

namespace TechTest.BancoMaster.Travels.Api.Models.Requests;

public class SearchCheapestTravelRequest
{
    [FromRoute]
    public string From { get; set; }

    [FromRoute]
    public string To { get; set; }
}
