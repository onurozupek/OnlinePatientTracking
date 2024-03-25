using AppointmentSystemMicroservice.DAL;
using AppointmentSystemMicroservice.Entities;

namespace AppointmentSystemMicroservice.DAL;

public class Initializer
{
    public static void CreateSeedData(IApplicationBuilder app)
    {
        using (var scope = app.ApplicationServices.CreateScope())
        {
            var context = scope.ServiceProvider.GetService<AppDbContext>();

            if (!context.Appointments.Any())
            {
                context.Appointments.AddRange(
                    new Appointment()
                    {
                        PatientName = "Test Microbiology Patient",
                        DoctorName = "Dr. Onur",
                        AppointmentDate = DateTime.Now,
                        Department = "Microbiology"
                    },
                    new Appointment()
                    {
                        PatientName = "Test Urology Patient",
                        DoctorName = "Dr. EBA",
                        AppointmentDate = DateTime.Now,
                        Department = "Urology"
                    },
                    new Appointment()
                    {
                        PatientName = "Test Immmunology Patient",
                        DoctorName = "Dr. Cosku",
                        AppointmentDate = DateTime.Now,
                        Department = "Immunology"
                    }
                );
            }

            context.SaveChanges();

        }
    }
}
