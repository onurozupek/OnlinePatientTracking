using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;

namespace ManagementMicroservice.Services;

public class MessageConsumer : IMessageConsumer
{
    public async Task<string> ConsumeMessage(string queueName)
    {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost",
            UserName = "user",
            Password = "password",
            VirtualHost = "/"
        };
        var messageReceived = new TaskCompletionSource<string>();
        var conn = factory.CreateConnection();
        using var channel = conn.CreateModel();
        channel.QueueDeclare(queueName, durable: true, exclusive: false);
        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, eventArgs) =>
        {
            // get byte[]
            var body = eventArgs.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            messageReceived.SetResult(message);

        };
        channel.BasicConsume(queueName, true, consumer);
        if (messageReceived.Task.Wait(2000))
        {
        return await messageReceived.Task;
        }
        return "";
    }
}
