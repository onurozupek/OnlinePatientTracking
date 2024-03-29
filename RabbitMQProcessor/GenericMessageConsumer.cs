using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQProcessor;

public class GenericMessageConsumer : IGenericMessageConsumer
{
    public event EventHandler<string> MessageReceived;
    public async Task ConsumeMessage(string queueName)
    {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost",
            UserName = "user",
            Password = "password",
            VirtualHost = "/"
        };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        channel.QueueDeclare(queueName, durable: true, exclusive: false);
        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, eventArgs) =>
        {
            var body = eventArgs.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine("Received message: {0}", message);
            ProcessMessage(queueName, message);
        };
        channel.BasicConsume(queueName, true, consumer);
        Console.WriteLine("Listening for messages...");
        while (true)
        {
            await Task.Delay(1000); // Wait for 1 second before checking again
        }
    }

    private async void ProcessMessage(string queueName, string message)
    {
        MessageReceived?.Invoke(queueName, message);
    }
}