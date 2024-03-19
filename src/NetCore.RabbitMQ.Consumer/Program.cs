using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using NetCore.Elasticsearch;
using Newtonsoft.Json;
using NetCore.Abstraction.Model;

const string CHANNEL_NAME = "log";
Console.WriteLine($"{DateTime.Now} RabbitMQ Started");
var elasticsearchRepo = new ElasticsearchRepository();

var factory = new ConnectionFactory()
{
    HostName = "localhost",
    UserName = "guest",
    Password = "guest"
};
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();
channel.QueueDeclare(queue: CHANNEL_NAME,
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);
var consumer = new EventingBasicConsumer(channel);
consumer.Received += async (model, args) =>
{
    var body = args.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    var logModel = JsonConvert.DeserializeObject<LogModel>(message);
    if (logModel is not null)
        await elasticsearchRepo.SetData(logModel);
    Console.WriteLine($"Received: {message}");
};
channel.BasicConsume(queue: CHANNEL_NAME,
                     autoAck: true,
                     consumer: consumer);



Console.ReadLine();