using Microsoft.Extensions.Options;
using NetCore.Abstraction;
using NetCore.Abstraction.Model;
using NetCore.Elasticsearch;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var appSettingsJson = File.ReadAllText("appsettings.json");
var settings = JObject.Parse(appSettingsJson);

var elasticOption = Options.Create(settings.SelectToken("Elasticsearch")!.ToObject<ElasticsearchSettings>()!);
var rabbitMqSetting = settings.SelectToken("RabbitMQ")!.ToObject<RabbitMQSettings>()!;
var elasticsearchRepo = new ElasticsearchRepository<LogModel>(elasticOption, Settings.LOG_INDEX_NAME);

var factory = new ConnectionFactory()
{
    HostName = rabbitMqSetting.HostName,
    UserName = rabbitMqSetting.Username,
    Password = rabbitMqSetting.Password
};
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();
channel.QueueDeclare(queue: Settings.LOG_CHANNEL_NAME,
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
    {
        var result = await elasticsearchRepo.InsertAsync(logModel);
        Console.WriteLine($"Elasticsearch send data: {(result ? "Successfull" : "Fail")}");
    }
    Console.WriteLine($"Received: {message}");
};
channel.BasicConsume(queue: Settings.LOG_CHANNEL_NAME,
                     autoAck: true,
                     consumer: consumer);

Console.ReadLine();
