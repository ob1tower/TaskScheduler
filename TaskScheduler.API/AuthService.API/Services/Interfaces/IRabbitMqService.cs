namespace AuthService.API.Services.Interfaces
{
    public interface IRabbitMqService : IDisposable
    {
        void ListenForMessages(Func<string, Task> onMessageReceived);
        void PublishMessage(string queueName, object message);
    }
}