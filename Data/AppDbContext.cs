using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskManagerApi.Models;

namespace TaskManagerApi.Data
{
    // This context class manages the connection to the database and sets up Identity + Tasks
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // This represents our TaskItem table
        public DbSet<TaskItem> Tasks { get; set; }
    }
}
