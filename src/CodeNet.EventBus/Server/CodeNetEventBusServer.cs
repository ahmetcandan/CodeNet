using CodeNet.EventBus.Client;
using CodeNet.EventBus.Models;
using CodeNet.Socket.EventDefinitions;
using CodeNet.Socket.Models;
using CodeNet.Socket.Server;

namespace CodeNet.EventBus.Server;

internal class CodeNetEventBusServer(int port) : CodeNetServer<CodeNetEventBusClient>(port)
{
    public event ClientConnectFinish<CodeNetEventBusClient>? ClientConnectFinish;

    public override string ApplicationKey => EventBusKey.ApplicationKey;

    protected override void ReceivedMessage(CodeNetEventBusClient client, Message message)
    {
        if (message.Type is (byte)Models.MessageType.SetChannel)
            ClientConnectFinish?.Invoke(new(client));
    }

    protected override void ClientConnecting(CodeNetEventBusClient client)
    {
        client.ClientConnectFinish += Client_ClientConnectFinish;
    }

    private void Client_ClientConnectFinish(ClientArguments<CodeNetEventBusClient> e)
    {
        ClientConnectFinish?.Invoke(e);
    }
}
