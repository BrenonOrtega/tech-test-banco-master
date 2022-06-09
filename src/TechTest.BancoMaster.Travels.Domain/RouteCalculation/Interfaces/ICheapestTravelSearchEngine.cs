using Awarean.Sdk.Result;

namespace TechTest.BancoMaster.Travels.Domain.RouteCalculation;

public interface ICheapestTravelSearchEngine
{
    Task<Result<ICheapestTravelResponse>> SearchAsync(ISearchTravelCommand travelConnection);
}
