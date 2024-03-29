namespace RabbitMQProcessor;

public interface IGenericMessageProducer
{
    public void SendingMessage<T>(T message, string queueName);
}
