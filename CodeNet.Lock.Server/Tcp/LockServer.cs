using System.Net;
using System.Net.Sockets;

namespace CodeNet.Lock.Server.Tcp;

internal class LockServer(int port)
{
    private readonly Core.LockManager _lockManager = new();
    private readonly int _port = port;
    private TcpListener? _tcpListener;
    private Thread? _thread;
    private bool _started = false;

    public void Start()
    {
        _tcpListener = new TcpListener(IPAddress.Any, _port);
        _tcpListener.Start();

        _thread = new Thread(new ThreadStart(ClientAccept));
        _thread.Start();

        _started = true;
    }

    public void Stop() => Stop(false);

    private void Stop(bool retryStart = false)
    {
        if (_started)
        {
            _tcpListener?.Stop();
            _started = false;
        }
        _thread?.Join();

        if (retryStart)
            Start();
    }

    private async void ClientAccept()
    {
        while (_started)
        {
            try
            {
                if (_tcpListener is not null)
                {
                    var tcpClient = await _tcpListener.AcceptTcpClientAsync();
                    if (tcpClient is not null)
                    {
                        using var stream = tcpClient.GetStream();
                        using var reader = new BinaryReader(stream);
                        using var writer = new BinaryWriter(stream);

                        var length = BitConverter.ToInt32(reader.ReadBytes(4));
                        var data = reader.ReadBytes(length - 4);
                        var request = LockRequest.Deseriliaze(data);

                        var result = _lockManager.TryAcquireLock(request.Key, request.ExpireDate);
                        if (result is not null)
                        {
                            var writerData = LockResult.Seriliaze(result);
                            writer.Write(writerData);
                        }
                        else
                        {
                            var writerData = LockResult.Seriliaze(new LockResult());
                            writer.Write(writerData);
                        }
                    }
                }
                else
                    Stop();
            }
            catch
            {
                Stop(true);
            }
        }
    }
}
