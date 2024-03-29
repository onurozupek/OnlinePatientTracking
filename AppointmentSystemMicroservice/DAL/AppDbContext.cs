using AppointmentSystemMicroservice.Entities;
using Microsoft.EntityFrameworkCore;

namespace AppointmentSystemMicroservice.DAL;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<PatientVisit> PatientVisits { get; set; }
}
