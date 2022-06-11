
namespace TechTest.BancoMaster.Travels.Domain.Travels.Contracts;
public interface ISearchTravelCommand
{
    public Location From { get; }
    public Location To { get; }
}