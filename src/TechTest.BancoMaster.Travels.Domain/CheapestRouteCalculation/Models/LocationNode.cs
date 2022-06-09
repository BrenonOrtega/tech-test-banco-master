using TechTest.BancoMaster.Travels.Domain.Structures;
using TechTest.BancoMaster.Travels.Domain.Travels;

namespace TechTest.BancoMaster.Travels.Domain.CheapestRouteCalculation;
public record LocationNode(Location Source) : Node<Location, decimal>(Source)
{
    public override string Id => Source;
}