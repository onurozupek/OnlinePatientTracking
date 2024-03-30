using Q_AMicroservice.Entities;

namespace Q_AMicroservice.DAL;

public class Initializer
{
    public static void CreateSeedData(IApplicationBuilder app)
    {
        using (var scope = app.ApplicationServices.CreateScope())
        {
            var context = scope.ServiceProvider.GetService<AppDbContext>();

            if (!context.Questions.Any())
            {
                context.Questions.AddRange(
    new Question()
    {
        Content = "What are the symptoms of Alzheimer's disease?",
        Answered = false,
        DoctorId = 1
    },
    new Question()
    {
        Content = "How can I prevent heart disease?",
        Answered = false,
        DoctorId = 2
    },
    new Question()
    {
        Content = "What are the common causes of food allergies?",
        Answered = false,
        DoctorId = 3
    }
);
                context.SaveChanges();
            }
        }
    }
}
