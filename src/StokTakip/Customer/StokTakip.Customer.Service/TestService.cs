namespace StokTakip.Customer.Service;

public class TestService
{
    public async Task TestWriteAsync() 
    {
        Console.WriteLine("<<<<<<<<<< TEST WRITE ASYNC >>>>>>>>>>");
        await Task.Delay(TimeSpan.FromSeconds(3));
    }

    public void TestWrite()
    {
        Console.WriteLine("<<<<<<<<<< TEST WRITE >>>>>>>>>>");
        Thread.Sleep(2000);
    }
}
