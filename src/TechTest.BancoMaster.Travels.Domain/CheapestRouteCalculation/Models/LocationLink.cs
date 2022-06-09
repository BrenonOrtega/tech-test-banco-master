using TechTest.BancoMaster.Travels.Domain.Structures;
using TechTest.BancoMaster.Travels.Domain.Travels;

namespace TechTest.BancoMaster.Travels.Domain.CheapestRouteCalculation;

public record LocationLink(Location Destination, decimal Weight) : Link<Location, decimal>(Destination, Weight);