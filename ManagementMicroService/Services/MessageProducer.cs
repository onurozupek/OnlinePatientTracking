using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace ManagementMicroservice.Services;

public class MessageProducer : IMessageProducer
{
    public void SendingMessage<T>(T message, string queueName)
    {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost",
            UserName = "user",
            Password = "password",
            VirtualHost = "/"
        };

        var conn = factory.CreateConnection();
        using var channel = conn.CreateModel();

        channel.QueueDeclare(queueName, durable: true, exclusive: false);

        var jsonString = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(jsonString);

        channel.BasicPublish("", queueName, body: body);

    }
}
