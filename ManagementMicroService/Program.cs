using ManagementMicroservice.DAL;
using ManagementMicroservice.Repositories;
using ManagementMicroservice.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RabbitMQProcessor;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Management API", Version = "v1" });
    c.DocInclusionPredicate((docName, apiDesc) =>
    {
        // Controller'� d��ar�da b�rakmak i�in ko�ullar burada belirtilir
        if (apiDesc.RelativePath.Contains("Appointment") || apiDesc.RelativePath.Contains("Patient"))
        {
            return false; // Controller belgelendirmeye dahil edilmez
        }
        return true;
    });
});
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionStr")), ServiceLifetime.Singleton);
builder.Services.AddSingleton<IGenericMessageProducer, GenericMessageProducer>();
builder.Services.AddSingleton(typeof(GenericRepository<>));
builder.Services.AddHostedService<MessageListener>();

var app = builder.Build();

Initializer.CreateSeedData(app);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
