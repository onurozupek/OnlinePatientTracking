using ManagementMicroservice.DAL;
using ManagementMicroservice.Repositories;
using ManagementMicroservice.Services;
using Microsoft.EntityFrameworkCore;
using RabbitMQConsumer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionStr")));
builder.Services.AddScoped<IGenericMessageProducer, GenericMessageProducer>();
builder.Services.AddScoped<IGenericMessageConsumer, GenericMessageConsumer>();
builder.Services.AddScoped(typeof(GenericRepository<>));
builder.Services.AddSingleton<MessageProcessor>();

var app = builder.Build();

Initializer.CreateSeedData(app);

var messageProcessor = app.Services.GetRequiredService<MessageProcessor>();

messageProcessor.StartProcessing("appointmentCreated");

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
