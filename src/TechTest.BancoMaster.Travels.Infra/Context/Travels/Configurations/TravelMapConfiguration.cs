
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechTest.BancoMaster.Travels.Infra.Context.Models;

namespace TechTest.BancoMaster.Travels.Infra.Context.Travels.Configurations;

class TravelMapConfiguration : IEntityTypeConfiguration<TravelModel>
{
    public void Configure(EntityTypeBuilder<TravelModel> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasMaxLength(40);
        builder.Property(x => x.StartingPoint).HasMaxLength(40).HasColumnType<string>("VARCHAR(40)");
        builder.Property(x => x.Destination).HasMaxLength(40).HasColumnType<string>("VARCHAR(40)");
        builder.Property(x => x.Amount).HasColumnType<decimal>("decimal(5, 2)");
        builder.Property(x => x.CreatedAt).HasColumnType<DateTime>("timestamp");
        builder.Property(x => x.UpdatedAt).HasColumnType<DateTime>("timestamp");
        builder.Property(x => x.UpdatedBy).HasColumnType<string?>("VARCHAR(60)").IsRequired(false);
    }
}