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
        consumerService.CheckSuccessfullMessage(args.DeliveryTag);
        await Task.Delay(new Random().Next(2000, 5000));
        //await Task.Delay(1000);
        stopwatch.Stop();
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] Output, ElapsedMilliseconds: {stopwatch.ElapsedMilliseconds}, {message}");
    }
}
