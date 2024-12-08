using CodeNet.EventBus.Services;
using CodeNet.Kafka.Services;
using CodeNet.RabbitMQ.Services;
using System.Text;

namespace CodeNet_App1.Services;

public class FireAndForget
{
    public static async Task Execute(IServiceProvider serviceProvider, int messagePerSecond, int second)
    {
        using var scope = serviceProvider.CreateScope();

        //var producerService = scope.ServiceProvider.GetService<RabbitMQProducerService>();
        //if (producerService is null)
        //    return;

        using var publisherService = scope.ServiceProvider.GetService<EventBusPublisherService>();
        if (publisherService is null)
            return;


        //var producerService = scope.ServiceProvider.GetRequiredService<KafkaProducerService>();
        List<byte[]> datas = new List<byte[]>(messagePerSecond);
        for (int i = 1; i <= messagePerSecond; i++)
        {
            datas.Add(BitConverter.GetBytes(i));
            //await Task.Run(() =>
            //{
            //    //publisherService.Publish(Encoding.UTF8.GetBytes($"[{second:000}] - [{i:000}] Message Time: {DateTime.Now:HH:mm:ss.fff}"));
            //    publisherService.Publish(BitConverter.GetBytes(i));
            //    //Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] [{second:000}] - [{i:000}] Message Published");
            //    Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] [{i}] Message Published");
            //    return Task.CompletedTask;
            //});
        }
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] Message Publish Started");
        publisherService.Publish(datas);
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] Message Publish Finished");
    }
}
