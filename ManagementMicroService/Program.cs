using ManagementMicroservice.DAL;
using ManagementMicroservice.Repositories;
using ManagementMicroservice.Services;
using Microsoft.EntityFrameworkCore;
using RabbitMQProcessor;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionStr")), ServiceLifetime.Singleton);
builder.Services.AddSingleton<IGenericMessageProducer, GenericMessageProducer>();
builder.Services.AddSingleton<IGenericMessageConsumer, GenericMessageConsumer>();
builder.Services.AddSingleton(typeof(GenericRepository<>));
//builder.Services.AddSingleton<MessageProcessor>();
builder.Services.AddHostedService<MessageListener>();

var app = builder.Build();

Initializer.CreateSeedData(app);


//var messageProcessor = app.Services.GetRequiredService<MessageProcessor>();

//// Start processing for each queue asynchronously
//var task1 = Task.Run(() => messageProcessor.StartProcessing("appointmentCreated"));
//var task2 = Task.Run(() => messageProcessor.StartProcessing("appointmentRemoved"));

//// Wait for both tasks to complete
//await Task.WhenAll(task1, task2);

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
