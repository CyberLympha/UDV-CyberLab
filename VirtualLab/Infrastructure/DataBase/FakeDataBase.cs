using Microsoft.EntityFrameworkCore;
using VirtualLab.Domain.Entities;

namespace VirtualLab.Infrastructure.DataBase;

public class LabDbContext : DbContext
{
    public DbSet<Lab> Labs { get; set; }
    public DbSet<UserLab> UserLabs { get; set; }
    public DbSet<VirtualMachine> VirtualMachines { get; set; }
    public DbSet<Credential> Credentials { get; set; }
    public DbSet<StatusUserLab> UserLabStatus { get; set; }
    
    public LabDbContext(DbContextOptions<LabDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StatusUserLab>()
            .Property(e => e.Name)
            .HasConversion<string>();

        modelBuilder.Entity<UserLab>()
            .HasOne(ul => ul.Report)
            .WithOne(r => r.UserLab)
            .HasForeignKey<Report>(r => r.UserLabId);

        modelBuilder.Entity<UserLab>()
            .HasOne(ul => ul.Lab)
            .WithMany(l => l.UserLabs)
            .HasForeignKey(ul => ul.LabId);

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //optionsBuilder.UseInMemoryDatabase("FakeDbContext");
        optionsBuilder.UseNpgsql("Host=158.160.91.137; Port=5432; Database=UdvLab; User ID=dev; Password=123123");
    }
}