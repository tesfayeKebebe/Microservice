using System.Text;
using System.Text.Json;
using System.Text.Unicode;
using PlatformService.Dtos;
using RabbitMQ.Client;

namespace PlatformService.AsyncDataServices;

public class MessageBusClient : IMessageBusClient, IDisposable
{
    private readonly IConfiguration _configuration;
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public MessageBusClient(IConfiguration configuration)
    {
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
            _channel.ExchangeDeclare(exchange:"trigger", type:ExchangeType.Fanout);
            _connection.ConnectionShutdown += RabbitMq_ConnectionShutdown;
            Console.WriteLine($"-->Connected to message bus");
        }
        catch (Exception e)
        {
            Console.WriteLine($"-->Could not connect to message bus {e.Message}");
        }
    }

    private void RabbitMq_ConnectionShutdown(object? sender, ShutdownEventArgs e)
    {
        Console.WriteLine($"-->Connection shutdown ");
    }

    private void SendMessage(string message)
    {
        var body = Encoding.UTF8.GetBytes(message);
        _channel.BasicPublish(exchange:"trigger",
            routingKey:"", basicProperties:null,
            body:body);
        Console.WriteLine($"We sent {message}");
    }
    public void PublishNewPlatform(PlatformPublishedDto publishedDto)
    {
        if (_connection.IsOpen)
        {
            Console.WriteLine("Sending message");
            var data = JsonSerializer.Serialize(publishedDto);
            SendMessage(data);
        }
        else
        {
            Console.WriteLine("RabbitMQ connection closed, Not sending anything");
        }
    }

    public void Dispose()
    {
        if (!_connection.IsOpen) return;
        _connection.Close();
        _channel.Close();
    }
}