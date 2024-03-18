using ManagementMicroService.Entities;

namespace ManagementMicroService.DAL;

public class Initializer
{
    public static void CreateSeedData(IApplicationBuilder app)
    {
        using (var scope = app.ApplicationServices.CreateScope())
        {
            var context = scope.ServiceProvider.GetService<AppDbContext>();

            if (!context.Departments.Any())
            {
                context.Departments.AddRange(
                    new Department()
                    {
                        Name = "Neurology",
                        Code = "N01",
                        Description = "",
                        Location = "B-1"
                    },
                    new Department()
                    {
                        Name = "Cardiology",
                        Code = "C01",
                        Description = "",
                        Location = "F-4"
                    },
                    new Department()
                    {
                        Name = "Allergy and Immunology",
                        Code = "A01",
                        Description = "Allergy",
                        Location = "F-1"
                    }
                );
            }

            context.SaveChanges();

        }
    }
}
