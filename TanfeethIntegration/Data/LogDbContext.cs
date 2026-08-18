using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TanfeethIntegration.Models;

namespace TanfeethIntegration.Data
{
    public class LogDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<RequestResponseLog> RequestResponseLogs { get; set; }

        public LogDbContext(DbContextOptions<LogDbContext> options) : base(options)
        {
        }

        // Optionally override the OnModelCreating method
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Define model relationships and configurations if needed
        }
    }
}
