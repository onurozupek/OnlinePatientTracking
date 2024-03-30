using Microsoft.EntityFrameworkCore;
using Q_AMicroservice.Entities;

namespace Q_AMicroservice.DAL
{
    public class AppDbContext : DbContext
    {
        public DbSet<Question> Questions { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
    }
}
