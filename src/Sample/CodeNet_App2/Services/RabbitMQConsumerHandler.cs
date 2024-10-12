using CodeNet.RabbitMQ.Models;
using CodeNet.RabbitMQ.Services;
using System.Diagnostics;
using System.Text;

namespace CodeNet_App2.Services;

public class RabbitMQConsumerHandler(RabbitMQConsumerService consumerService) : IRabbitMQConsumerHandler<RabbitMQConsumerService>
{
    public async Task Handler(ReceivedMessageEventArgs args)
    {
        var message = Encoding.UTF8.GetString(args.Data.ToArray());
        
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] Input {message}, Timestamp: {args.Timestamp}");
        Stopwatch stopwatch = Stopwatch.StartNew();
        await Task.Delay(1000);
        consumerService.CheckSuccessfullMessage(args.DeliveryTag);
        stopwatch.Stop();
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] Output, ElapsedMilliseconds: {stopwatch.ElapsedMilliseconds}, {message}");
    }
}
