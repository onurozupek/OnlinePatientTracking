namespace ManagementMicroservice.Services;

public interface IMessageConsumer
{
    public Task<string> ConsumeMessage(string queueName);
}
