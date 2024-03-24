using Microsoft.Extensions.Options;
using NetCore.Abstraction;
using NetCore.Abstraction.Model;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace NetCore.RabbitMQ;

public class RabbitMQService(IOptions<RabbitMQSettings> Config) : IQService
{
    public bool Post(string channelName, object data)
    {
        try
        {
            var factory = new ConnectionFactory()
            {
                HostName = Config.Value.HostName,
                UserName = Config.Value.Username,
                Password = Config.Value.Password
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
