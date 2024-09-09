using CodeNet.Kafka.Services;
using Confluent.Kafka;
using System.Diagnostics;

namespace CodeNet_App2.Services;

public class KafkaConsumerHandler(KafkaConsumerService consumerService) : IKafkaConsumerHandler<KafkaConsumerService>
{

    public async Task Handler(CodeNet.Kafka.Models.ReceivedMessageEventArgs<Null, string> args)
    {
        var message = args.Value;
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] Input {message}");
        Stopwatch stopwatch = Stopwatch.StartNew();
        //await Task.Delay(new Random().Next(250, 1000));
        await Task.Delay(250);
        consumerService.CommitCheckPoint(args.Partition, args.Offset);
        stopwatch.Stop();
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] Output, ElapsedMilliseconds: {stopwatch.ElapsedMilliseconds}, {message}");
    }
}
