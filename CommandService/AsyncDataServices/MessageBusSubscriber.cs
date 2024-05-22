using System.Text;
using CommandService.EventProcessors;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CommandService.AsyncDataServices;

public class MessageBusSubscriber: BackgroundService, IDisposable
{
    private readonly IConfiguration _configuration;
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly string _queueName;
    private readonly IEventProcessor _eventProcessor;

    public MessageBusSubscriber(IConfiguration configuration, IEventProcessor eventProcessor)
    {
        _eventProcessor = eventProcessor;
        _configuration = configuration;
        var factory = new ConnectionFactory()
        {
            HostName = _configuration["RabbitMQHost"],
            Port = int.Parse(_configuration["RabbitMQPort"])
        };
        try
        {
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
            _queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(_queueName,exchange: "trigger", routingKey:"");
            _connection.ConnectionShutdown += RabbitMq_ConnectionShutdown;

            Console.WriteLine("--> Listening to the message bus");
        }
        catch (Exception e)
        {
            Console.WriteLine("-->  Not listening to the message bus");
        }
    }

    private void RabbitMq_ConnectionShutdown(object? sender, ShutdownEventArgs e)
    {
        Console.WriteLine($"-->Connection shutdown ");
    }
    
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (ModuleHandle, ea) =>
        {
            Console.WriteLine("--> Event received");
            var body = ea.Body;
            var notificationMessage = Encoding.UTF8.GetString(body.ToArray());
            _eventProcessor.ProcessEvent(notificationMessage);
        };
        _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);
        return Task.CompletedTask;
    }
    public void Dispose()
    {
        if (!_connection.IsOpen) return;
        _connection.Close();
        _channel.Close();
    }
}