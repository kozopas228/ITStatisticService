using Microsoft.EntityFrameworkCore;

namespace ITStatisticService.Data
{
    public sealed class ApplicationContext : DbContext
    {
        public DbSet<LoggingResult> Results { get; set; }
        
        public ApplicationContext()
        {
            Database.EnsureCreated();
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=Statistic.db");
        }
    }
}