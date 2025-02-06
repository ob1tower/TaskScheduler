using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace NoteService.API.Services;

public class RabbitMqService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly ILogger<RabbitMqService> _logger;

    public RabbitMqService(string hostName, ILogger<RabbitMqService> logger)
    {
        var factory = new ConnectionFactory { HostName = hostName };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.QueueDeclare(queue: "NoteService",
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);
        _logger = logger;
    }

    public void PublishMessage(string queueName, object message)
    {
        var body = Encoding.UTF8.GetBytes(System.Text.Json.JsonSerializer.Serialize(message));

        _logger.LogInformation($"[RabbitMQ] Отправка сообщения в очередь '{queueName}'.");

        _channel.BasicPublish(exchange: "",
                              routingKey: queueName,
                              basicProperties: null,
                              body: body);

        _logger.LogInformation($"[RabbitMQ] Сообщение отправлено в очередь '{queueName}'.");
    }

    public void ListenForMessages(Func<string, Task> onMessageReceived)
    {
        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            _logger.LogInformation($"[RabbitMQ] Получено сообщение: {message}");

            await onMessageReceived(message);

            _channel.BasicAck(ea.DeliveryTag, false);
            _logger.LogInformation($"[RabbitMQ] Сообщение обработано: {message}");
        };

        _logger.LogInformation("[RabbitMQ] Начато потребление сообщений из очереди 'NoteService'.");
        _channel.BasicConsume(queue: "NoteService", autoAck: false, consumer: consumer);
    }

    public void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
    }
}
