using Microsoft.EntityFrameworkCore;
using ProxmoxApi.Domen.Entities;
using VirtualLab.Domain.Entities;

namespace VirtualLab.Infrastructure.DataBase;

public class FakeDbContext : DbContext
{
    public DbSet<Lab> Labs { get; set; }
    public DbSet<UserLab> UserLabs { get; set; }
    public FakeDbContext(DbContextOptions<FakeDbContext> options) : base(options)
    {
      
    }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("FakeDbContext");
    }
}