using Microsoft.EntityFrameworkCore;
using TechTest.BancoMaster.Travels.Infra.Context.Models;
using TechTest.BancoMaster.Travels.Infra.Context.Travels.Configurations;

namespace TechTest.BancoMaster.Travels.Infra;

public class TravelsContext : DbContext
{
    internal DbSet<TravelModel> Travels { get; set; }

    public TravelsContext()
    {
    }

    public TravelsContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        if (builder.IsConfigured is false)
            builder.UseNpgsql("mock-string-for-design-Time-Migrations");

        base.OnConfiguring(builder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new TravelMapConfiguration());

        base.OnModelCreating(builder);
    }
}