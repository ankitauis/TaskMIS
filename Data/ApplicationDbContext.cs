using TaskMIS.Models;
using Microsoft.EntityFrameworkCore;
using TaskMIS.Models;

namespace TaskMIS.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<TaskEntity> Tasks{ get; set; }

         public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskEntity>()
                .HasOne(t => t.User) 
                .WithMany(u => u.Tasks) 
                .HasForeignKey(t => t.UserId);
        }
    }
}
