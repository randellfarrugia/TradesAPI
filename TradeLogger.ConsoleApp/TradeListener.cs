using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace TradeLogger.ConsoleApp
{
    public class TradeListener
    {
        private readonly string _hostname = "rabbitmq";
        private readonly string _queueName = "trades";
        private readonly string _username = "guest";
        private readonly string _password = "guest";
        private readonly int _retryDelayMs = 3000;

        public async Task ListenForTrades()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _hostname,
                Port = 5672,
                UserName = _username,
                Password = _password
            };

            IChannel channel = await InitConnection(factory);

            while (true)
            {
                try
                {
                    await channel.QueueDeclareAsync(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

                    var consumer = new AsyncEventingBasicConsumer(channel);

                    consumer.ReceivedAsync += async (sender, e) =>
                    {
                        var body = e.Body;
                        var message = Encoding.UTF8.GetString(body.ToArray());
                        Console.WriteLine($"Received message: {message}");
                        await Task.Yield();
                    };

                    await channel.BasicConsumeAsync(queue: _queueName, autoAck: true, consumer: consumer);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to connect to RabbitMQ. Retrying... Error: {ex.Message}");
                    await Task.Delay(_retryDelayMs);

                    await InitConnection(factory);
                }
            }
        }

        private async Task<IChannel> InitConnection(ConnectionFactory factory)
        {
            bool connected = false;
            IChannel channel = null;
            while (!connected)
            {
                try
                {
                    IConnection conn = await factory.CreateConnectionAsync();
                    channel = await conn.CreateChannelAsync();
                    Console.WriteLine("Connected to RabbitMQ and listening for messages...");
                    connected = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to connect to RabbitMQ. Retrying... Error: {ex.Message}");
                    await Task.Delay(_retryDelayMs);
                }
            }

            return channel;
        }
    }
}
