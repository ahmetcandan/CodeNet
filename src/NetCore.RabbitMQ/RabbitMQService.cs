using Microsoft.Extensions.Options;
using NetCore.Abstraction;
using NetCore.Abstraction.Model;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace NetCore.RabbitMQ;

public class RabbitMQService(IOptions<RabbitMQSettings> config) : IQService
{
    public readonly IOptions<RabbitMQSettings> config = config;

    public bool Post(string channelName, object data)
    {
        try
        {
            var factory = new ConnectionFactory()
            {
                HostName = config.Value.HostName,
                UserName = config.Value.Username,
                Password = config.Value.Password
            };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(queue: channelName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
            string message = JsonConvert.SerializeObject(data);
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "",
                                 routingKey: channelName,
                                 basicProperties: null,
                                 body: body);
            Console.WriteLine($"RabbitMQ Channel: {channelName}, Send: {message}");
            return true;
        }
        catch
        {
            return false;
        }
    }
}
