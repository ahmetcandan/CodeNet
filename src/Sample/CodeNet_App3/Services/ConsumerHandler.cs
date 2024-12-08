using CodeNet.EventBus.Services;
using CodeNet.RabbitMQ.Services;
using System.Diagnostics;
using System.Text;

namespace CodeNet_App3.Services;

public class ConsumerHandler(RabbitMQConsumerService consumerService) : IRabbitMQConsumerHandler<RabbitMQConsumerService>
{
    public async Task Handler(CodeNet.RabbitMQ.Models.ReceivedMessageEventArgs args)
    {
        var message = Encoding.UTF8.GetString(args.Data.ToArray());
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] Input {message}");
        Stopwatch stopwatch = Stopwatch.StartNew();
        await Task.Delay(new Random().Next(250, 1000));
        consumerService.CheckSuccessfullMessage(args.DeliveryTag);
        stopwatch.Stop();
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] Output, ElapsedMilliseconds: {stopwatch.ElapsedMilliseconds}, {message}");
    }
}

public class EventBusSubscriberHandler : IEventBusSubscriberHandler<EventBusSubscriberService>
{
    public void Handler(ReceivedMessageEventArgs args)
    {
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] EventBus.Subscriber, {BitConverter.ToInt32(args.Message)}");
    }
}
