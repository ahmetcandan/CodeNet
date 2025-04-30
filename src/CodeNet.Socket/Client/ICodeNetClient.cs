using System.Net.Sockets;

namespace CodeNet.Socket.Client;

internal interface ICodeNetClient : IDisposable
{
    void SetTcpClient(TcpClient client, ulong clientId);
}