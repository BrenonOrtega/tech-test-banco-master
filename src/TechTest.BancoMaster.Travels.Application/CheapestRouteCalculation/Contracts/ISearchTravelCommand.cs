using TechTest.BancoMaster.Travels.Domain.Travels;

namespace TechTest.BancoMaster.Travels.Application.CheapestRouteCalculation.Contracts;
public interface ISearchTravelCommand
{
    public Location From { get; }
    public Location To { get; }
}