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
        var envUrl = System.Environment.GetEnvironmentVariable("DB_LABS_URL");
        var host = "localhost";
        var port = "5432";
        if (envUrl != null && envUrl != string.Empty)
        {
            var envUrlParts = envUrl.Split(':');
            host = envUrlParts[1].Substring(2);
            port = envUrlParts[2];
        }
        optionsBuilder.UseNpgsql($"Host={host}; Port={port}; Database=Labs; User ID=dev; Password=123123");
    }
}