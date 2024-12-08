using RabbitMQ.Client;
using System.Text;

namespace TradesApi.Infrastructure.Services
{
    public class MessageQueueService
    {
        private readonly string _hostname = "rabbitmq";
        private readonly string _queueName = "trades";
        private readonly string _username = "guest";
        private readonly string _password = "guest";

        public async void SendTradeMessage(string message)
        {
            var factory = new ConnectionFactory() { HostName = _hostname, UserName = _username, Password = _password };
            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var body = Encoding.UTF8.GetBytes(message);

            await channel.BasicPublishAsync(exchange: "", routingKey: _queueName, body: body);
        }
    }
}