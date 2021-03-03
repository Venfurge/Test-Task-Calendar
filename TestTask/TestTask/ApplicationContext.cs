using Microsoft.EntityFrameworkCore;
using TestTask.Entities;

namespace TestTask
{
    public class ApplicationContext : DbContext
    {
        public DbSet<DayEntity> Days { get; set; }
        public DbSet<TaskEntity> Tasks { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Creating Unique Date field for Days
            modelBuilder.Entity<DayEntity>()
                .HasIndex(v => v.Date)
                .IsUnique();
        }
    }
}
