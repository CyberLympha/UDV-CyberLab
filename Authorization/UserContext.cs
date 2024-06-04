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
            optionsBuilder.UseNpgsql("Host=158.160.91.137; Port=5433; Database=UdvLab; User ID=dev; Password=123123");
            //optionsBuilder.UseInMemoryDatabase("AuthFakeContext");
        }
    }
}
