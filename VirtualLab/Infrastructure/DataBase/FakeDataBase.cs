using Microsoft.EntityFrameworkCore;
using VirtualLab.Domain.Entities;

namespace VirtualLab.Infrastructure.DataBase;

public class LabDbContext : DbContext
{
    public LabDbContext(DbContextOptions<LabDbContext> options) : base(options)
    {
    }

    public DbSet<Lab> Labs { get; set; }
    public DbSet<UserLab> UserLabs { get; set; }
    public DbSet<VirtualMachine> VirtualMachines { get; set; }
    public DbSet<Credential> Credentials { get; set; }
    public DbSet<StatusUserLab> UserLabStatus { get; set; }
    public DbSet<TemplateVm> TemplateVms { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StatusUserLab>()
            .Property(e => e.Name)
            .HasConversion<string>();


        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("FakeDbContext");
    }
}