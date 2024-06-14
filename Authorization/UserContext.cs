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
            var envUrl = System.Environment.GetEnvironmentVariable("DB_AUTH_URL");
            var host = "localhost";
            var port = "5433";
            if (envUrl != null && envUrl != string.Empty)
            {
                var envUrlParts = envUrl.Split(':');
                host = envUrlParts[1].Substring(2);
                port = envUrlParts[2];
            }
            optionsBuilder.UseNpgsql($"Host={host}; Port={port}; Database=Auth; User ID=user; Password=123123");
            //optionsBuilder.UseInMemoryDatabase("AuthFakeContext");
        }
    }
}
