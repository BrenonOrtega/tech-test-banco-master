using TechTest.BancoMaster.Travels.Domain.Structures;

namespace TechTest.BancoMaster.Travels.Application.CheapestRouteCalculation.Models;

public record LocationLink(string StartingPoint, string Destination, decimal Weight) : Link(StartingPoint, Destination, Weight);
