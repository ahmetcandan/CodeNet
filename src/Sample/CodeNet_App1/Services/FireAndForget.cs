﻿using CodeNet.Kafka.Services;
using CodeNet.RabbitMQ.Services;

namespace CodeNet_App1.Services;

public class FireAndForget
{
    public static async Task Execute(IServiceProvider serviceProvider, int messagePerSecond, int second)
    {
        using var scope = serviceProvider.CreateScope();
        //var producerService = scope.ServiceProvider.GetRequiredService<RabbitMQProducerService>();
        var producerService = scope.ServiceProvider.GetRequiredService<KafkaProducerService>();
        for (int i = 1; i <= messagePerSecond; i++)
        {
            await Task.Run(async () =>
            {
                await producerService.Publish(null, $"[{second:000}] - [{i:000}] Message");
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] [{second:000}] - [{i:000}] Message Published");
            });
        }
    }
}
