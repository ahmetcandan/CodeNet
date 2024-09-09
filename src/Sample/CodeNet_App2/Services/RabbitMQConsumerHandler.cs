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
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] Input {message}");
        Stopwatch stopwatch = Stopwatch.StartNew();
        //await Task.Delay(new Random().Next(250, 1000));
        await Task.Delay(250);
        consumerService.CheckSuccessfullMessage(args.DeliveryTag);
        stopwatch.Stop();
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] Output, ElapsedMilliseconds: {stopwatch.ElapsedMilliseconds}, {message}");
    }
}
