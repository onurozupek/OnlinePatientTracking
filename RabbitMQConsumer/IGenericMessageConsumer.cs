namespace RabbitMQConsumer;

public interface IGenericMessageConsumer
{
    public void ConsumeMessage(string queueName);
}
