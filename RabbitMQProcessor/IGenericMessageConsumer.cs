namespace RabbitMQProcessor;

public interface IGenericMessageConsumer
{
public Task ConsumeMessage(string queueName);
public event EventHandler<string> MessageReceived;
}