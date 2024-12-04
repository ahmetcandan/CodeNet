using System.Net.Sockets;

namespace CodeNet.EventBus.Client;

internal interface ICodeNetClient
{
    void SetTcpClient(TcpClient client, int clientId);
}