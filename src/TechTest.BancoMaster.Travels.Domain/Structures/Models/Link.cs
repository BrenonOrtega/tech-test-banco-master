namespace TechTest.BancoMaster.Travels.Domain.Structures;

public record Link<TNode, TWeight>(TNode Destination, TWeight Weight)
{

}