using AppointmentSystemMicroservice.DAL;
using ManagementMicroservice.DAL;
using ManagementMicroservice.Repositories;
using Microsoft.EntityFrameworkCore;
using RabbitMQConsumerAPI.Consumer;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var connectionString = ConfigurationHelper.GetConnectionString(configuration, "DefaultConnection");

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ManagementMicroservice.DAL.AppDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddScoped(typeof(GenericRepository<>));
builder.Services.AddScoped<IGenericMessageConsumer, GenericMessageConsumer>();

var app = builder.Build();

var consumer = app.Services.GetRequiredService<IGenericMessageConsumer>();

var task1 = Task.Run(() => consumer.ConsumeMessage("appointmentCreated"));
var task2 = Task.Run(() => consumer.ConsumeMessage("appointmentRemoved"));

// Wait for both tasks to complete
await Task.WhenAll(task1, task2);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
