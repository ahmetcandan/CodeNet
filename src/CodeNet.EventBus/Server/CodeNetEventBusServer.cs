using CodeNet.EventBus.Client;
using CodeNet.EventBus.EventDefinitions;
using CodeNet.EventBus.Models;

namespace CodeNet.EventBus.Server;

internal class CodeNetEventBusServer(int port) : CodeNetServer<CodeNetEventBusClient>(port)
{
    public event ClientConnectFinish<CodeNetEventBusClient>? ClientConnectFinish;

    internal override void ReceivedMessage(CodeNetEventBusClient client, Message message)
    {
        if (message.Type is MessageType.SetChannel)
            ClientConnectFinish?.Invoke(new(client));
    }

    internal override void ClientConnecting(CodeNetEventBusClient client)
    {
        client.ClientConnectFinish += Client_ClientConnectFinish;
    }

    private void Client_ClientConnectFinish(ClientArguments<CodeNetEventBusClient> e)
    {
        ClientConnectFinish?.Invoke(e);
    }
}
