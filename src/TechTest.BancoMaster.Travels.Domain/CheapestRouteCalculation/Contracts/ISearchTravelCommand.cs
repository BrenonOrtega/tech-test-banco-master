using TechTest.BancoMaster.Travels.Domain.Travels;

namespace TechTest.BancoMaster.Travels.Domain.CheapestRouteCalculation;
public interface ISearchTravelCommand
{
    public Location From { get; }
    public Location To { get; }
}