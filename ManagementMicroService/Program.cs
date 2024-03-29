using ManagementMicroservice.DAL;
using ManagementMicroservice.Repositories;
using ManagementMicroservice.Services;
using Microsoft.EntityFrameworkCore;
using RabbitMQProcessor;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
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
