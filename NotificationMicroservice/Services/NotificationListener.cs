using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQProcessor;
using System.Text;

namespace NotificationMicroservice.Services;

public class NotificationListener : BackgroundService
{
    public event EventHandler<string> MessageReceived;
    private readonly ILogger<NotificationListener> _logger;
    private readonly IGenericMessageProducer _producer;
    public NotificationListener(ILogger<NotificationListener> logger, IGenericMessageProducer producer)
    {
        _logger = logger;
        _producer = producer;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory
        {
            HostName = "localhost",
            UserName = "user",
            Password = "password",
            VirtualHost = "/",
            DispatchConsumersAsync = true
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();


        channel.QueueDeclare(queue: "notificationMessage",
                             durable: true,
                             exclusive: false);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            _logger.LogInformation("Received message from queue {0}: {1}", "notificationMessage", message);
            _producer.SendingMessage(message, "sendedNotification");
            OnMessageReceived(message);
            await Task.Yield();
        };

        channel.BasicConsume(queue: "notificationMessage",
                             autoAck: true,
                             consumer: consumer);

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
    }

    protected virtual void OnMessageReceived(string message)
    {
        MessageReceived?.Invoke(this, message);
    }
}
