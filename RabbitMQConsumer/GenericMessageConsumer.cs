using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQConsumer;

public class GenericMessageConsumer : IGenericMessageConsumer
{
    public event EventHandler<string> MessageReceived;
    public void ConsumeMessage(string queueName)
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
            OnMessageReceived(message);
        };
        channel.BasicConsume(queueName, true, consumer);
        Console.WriteLine("Listening for messages...");
        Console.ReadLine();
    }

    protected virtual void OnMessageReceived(string message)
    {
        MessageReceived?.Invoke(this, message);
    }
}