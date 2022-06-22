using System.Reflection;
using Awarean.Sdk.SharedKernel;
using TechTest.BancoMaster.Travels.Domain.Travels;

namespace TechTest.BancoMaster.Travels.Infra.Context.Models;

class TravelModel : AuditableEntity<string>
{
    public string StartingPoint { get; set; }

    public string Destination { get; set; }

    private decimal amount;
    public decimal Amount { get => amount; }

    public void NewAmount(decimal amount, string updater)
    {
        this.amount = amount;
        Update(updater);
    }

    public Travel ToDomainEntity() => (Travel)this;
    public static implicit operator Travel(TravelModel model) => new Travel(model.StartingPoint, model.Destination, model.Amount);
    public static implicit operator TravelModel(Travel model) => new TravelModel
    {
        Id = model.Id,
        Destination = model.Connection.Destination,
        StartingPoint = model.Connection.StartingPoint,
        CreatedAt = DateTime.Now,
        UpdatedAt = DateTime.Now,
        amount = model.Amount,
    };
}