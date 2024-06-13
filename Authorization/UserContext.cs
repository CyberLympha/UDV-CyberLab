using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Authorization
{
    public class UserContext : IdentityDbContext<User>
    {
        public UserContext(DbContextOptions options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var env = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var host = env == "Docker-Development" ? "host.docker.internal" : "localhost";
            optionsBuilder.UseNpgsql($"Host={host}; Port=5433; Database=Auth; User ID=user; Password=123123");
            //optionsBuilder.UseInMemoryDatabase("AuthFakeContext");
        }
    }
}
