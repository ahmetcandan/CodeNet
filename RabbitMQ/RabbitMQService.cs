using Net5Api.Abstraction;
using Net5Api.Abstraction.Model;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;

namespace Net5Api.RabbitMQ
{
    public class RabbitMQService : IQService
    {
        public readonly Microsoft.Extensions.Options.IOptions<RabbitMQSettings> config;
        public RabbitMQService(Microsoft.Extensions.Options.IOptions<RabbitMQSettings> config)
        {
            this.config = config;
        }

        public bool Post(string channelName, object data)
        {
            try
            {
                var factory = new ConnectionFactory() { HostName = config.Value.HostName };
                using (IConnection connection = factory.CreateConnection())
                using (IModel channel = connection.CreateModel())
                {
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
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
