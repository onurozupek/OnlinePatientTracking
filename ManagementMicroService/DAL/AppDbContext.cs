using ManagementMicroservice.Entities;
using Microsoft.EntityFrameworkCore;

namespace ManagementMicroservice.DAL;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
}
