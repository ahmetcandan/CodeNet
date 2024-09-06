using CodeNet.RabbitMQ.Models;
using CodeNet.RabbitMQ.Services;
using System.Diagnostics;
using System.Text;

namespace CodeNet_App2.Services;

public class ConsumerHandler : IRabbitMQConsumerHandler<RabbitMQConsumerService>
{
    public async Task Handler(ReceivedMessageEventArgs args)
    {
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] Input {Encoding.UTF8.GetString(args.Data.ToArray())}");
        Stopwatch stopwatch = Stopwatch.StartNew();
        await Task.Delay(500);
        stopwatch.Stop();
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] Output, ElapsedMilliseconds: {stopwatch.ElapsedMilliseconds}, {Encoding.UTF8.GetString(args.Data.ToArray())}");
    }

    public async Task Test()
    {
        //SuperSocket.Connection.TcpPipeConnection tcp = new SuperSocket.Connection.TcpPipeConnection()
    }
}
