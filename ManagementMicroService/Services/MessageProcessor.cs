using RabbitMQConsumer;

namespace ManagementMicroservice.Services
{
    public class MessageProcessor
    {
        private readonly GenericMessageConsumer _messageConsumer;

        public MessageProcessor()
        {
            _messageConsumer = new GenericMessageConsumer();
            _messageConsumer.MessageReceived += ProcessMessage;
        }

        public void StartProcessing(string queueName)
        {
            Console.WriteLine($"Starting message consumption from queue: {queueName}");
            _messageConsumer.ConsumeMessage(queueName);
        }

        private void ProcessMessage(object sender, string message)
        {
            Console.WriteLine(message);
            // Mesajı işleme kodları burada
        }
    }
}
