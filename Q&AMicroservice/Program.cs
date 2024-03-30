using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Q_AMicroservice.DAL;
using RabbitMQProcessor;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddHostedService<AnswerListener>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Q&A API", Version = "v1" });
});
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionStr")), ServiceLifetime.Singleton);
builder.Services.AddSingleton<IGenericMessageProducer, GenericMessageProducer>();
builder.Services.AddSingleton<AnswerListener>();

var app = builder.Build();

Initializer.CreateSeedData(app);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
