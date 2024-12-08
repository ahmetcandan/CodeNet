using System.Net.Sockets;

namespace CodeNet.EventBus.Client;

internal interface ICodeNetClient : IDisposable
{
    void SetTcpClient(TcpClient client, int clientId);
}