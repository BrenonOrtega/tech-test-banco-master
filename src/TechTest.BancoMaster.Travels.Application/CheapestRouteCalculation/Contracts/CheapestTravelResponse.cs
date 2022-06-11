using System.Text;
using Awarean.Sdk.ValueObjects;
using TechTest.BancoMaster.Travels.Domain.Travels;
using TechTest.BancoMaster.Travels.Domain.Travels.Contracts;

namespace TechTest.BancoMaster.Travels.Application.CheapestRouteCalculation.Contracts
{
    public class CheapestTravelResponse : ICheapestTravelResponse
    {
        public Location StartingPoint { get; private set; }
        public Location Destination { get; private set; }
        public Money TotalAmount { get; private set; }
        public LinkedList<(Location Location, Money Amount)> BestTravelRoute { get; private set; }

        public CheapestTravelResponse(Location startingPoint, Location destination, Money totalAmount, LinkedList<(Location Location, Money Amount)> bestTravelRoute)
        {
            StartingPoint = startingPoint ?? throw new ArgumentNullException(nameof(startingPoint));
            Destination = destination ?? throw new ArgumentNullException(nameof(destination));
            TotalAmount = totalAmount ?? throw new ArgumentNullException(nameof(totalAmount));
            BestTravelRoute = bestTravelRoute ?? throw new ArgumentNullException(nameof(bestTravelRoute));
        }

        public string DescribeCheapestTravel()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append($"Cheapest Travel Route from {StartingPoint} to {BestTravelRoute?.Last?.Value.Location} is:\n");

            var node = BestTravelRoute.First;
            do
            {
                stringBuilder.Append($"{node.Value.Location} -> ");
                node = node.Next;
            } while (node.Next is not null);

            stringBuilder.Append("\n");
            stringBuilder.Append($"Costing a total of {TotalAmount:C2}.");

            return stringBuilder.ToString();
        }
    }
}