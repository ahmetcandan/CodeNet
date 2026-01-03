using System.Net.Sockets;

namespace CodeNet.Lock
{
    public class LockClient(string serverIp, int serverPort)
    {
        private readonly string _serverIp = serverIp;
        private readonly int _serverPort = serverPort;

        public LockResult AcquireLock(string key, DateTime expireDate)
        {
            try
            {
                using var client = new TcpClient(_serverIp, _serverPort);
                using var stream = client.GetStream();
                using var writer = new BinaryWriter(stream);
                using var reader = new BinaryReader(stream);

                writer.Write(LockRequest.Seriliaze(new LockRequest(key, expireDate)));

                var length = BitConverter.ToInt32(reader.ReadBytes(4));
                var data = reader.ReadBytes(length - 4);
                return LockResult.Deseriliaze(data);
            }
            catch (Exception ex)
            {
                return new LockResult
                {
                    IsSuccess = false,
                    Message = $"Connection error: {ex.Message}"
                };
            }
        }

        public LockResult AcquireLock(string key, int timeoutMs) => AcquireLock(key, DateTime.UtcNow.AddMilliseconds(timeoutMs));
    }
}