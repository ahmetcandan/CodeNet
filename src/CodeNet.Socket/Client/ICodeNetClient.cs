using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;

namespace CodeNet.Socket.Client;

internal interface ICodeNetClient : IDisposable
{
    void SetTcpClient(TcpClient client, ulong clientId);
    void SetTcpClient(TcpClient client, ulong clientId, X509Certificate2 certificate);
}