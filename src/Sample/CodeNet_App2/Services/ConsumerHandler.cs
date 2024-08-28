using CodeNet.RabbitMQ.Models;
using CodeNet.RabbitMQ.Services;
using System.Text;

namespace CodeNet_App2.Services;

public class ConsumerHandler : IRabbitMQConsumerHandler<RabbitMQConsumerService>
{
    public async Task Handler(ReceivedMessageEventArgs args)
    {
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] {Encoding.UTF8.GetString(args.Data.ToArray())}");
        await Task.Delay(250);
    }
}
