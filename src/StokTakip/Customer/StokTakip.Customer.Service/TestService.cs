using CodeNet.BackgroundJob.Manager;

namespace StokTakip.Customer.Service;

public class TestService1 : IScheduleJob
{
    public async Task Execute(CancellationToken cancellationToken)
    {
        Console.WriteLine("<<<<<<<<<< TEST 1 WRITE ASYNC >>>>>>>>>>");
        await Task.Delay(TimeSpan.FromSeconds(15), cancellationToken);
    }
}

public class TestService2 : IScheduleJob
{
    public async Task Execute(CancellationToken cancellationToken)
    {
        Console.WriteLine("<<<<<<<<<< TEST 2 WRITE ASYNC >>>>>>>>>>");
        await Task.Delay(TimeSpan.FromSeconds(20), cancellationToken);
    }
}
