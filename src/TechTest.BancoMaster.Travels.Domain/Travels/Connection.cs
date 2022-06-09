using Awarean.Sdk.SharedKernel;

namespace TechTest.BancoMaster.Travels.Domain.Travels
{
    public class Connection : Entity<string>
    {
        public new string Id => $"{StartingPoint}#{Destination}";
        public Location StartingPoint { get; private set; }
        public Location Destination { get; private set; }
        public string FromTo => $"{StartingPoint} -> {Destination}";

        private Connection() { }
        public Connection(string startingPoint, string destination) : this((Location)startingPoint, (Location)destination) { }
        public Connection(Location startingPoint, Location destination) => (StartingPoint, Destination) = (startingPoint, destination);

        public static readonly Connection Null = new() { StartingPoint = Location.Null, Destination = Location.Null };
    }
}