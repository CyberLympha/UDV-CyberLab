using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using ProxmoxApi.Domen.Entities;
using VirtualLab.Domain.Entities;
using Guid = VirtualLab.Domain.Entities.Guid;

namespace VirtualLab.Infrastructure.DataBase;

public class FakeDbContext : DbContext
{
    public DbSet<Guid> Labs { get; set; }
    public DbSet<UserLab> UserLabs { get; set; }
    public FakeDbContext(DbContextOptions<FakeDbContext> options) : base(options)
    {
      
    }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("FakeDbContext");
    }
}