using CodeNet.EventBus.Client;

namespace CodeNet.EventBus.EventDefinitions;

public class ClientArguments<TClient>(TClient client)
    where TClient : CodeNetClient
{
    public TClient Client { get; } = client;
    public DateTime Date { get; } = DateTime.Now;
}
