using System.Text;

namespace CodeNet.Lock;

public class LockRequest
{
    private const int _defaultLength = 12;

    public string Key { get; init; }
    public DateTime ExpireDate { get; init; }

    public LockRequest(string key, DateTime expireDate)
    {
        Key = key;
        ExpireDate = expireDate;
    }

    public LockRequest(string key, TimeSpan lockTime)
    {
        Key = key;
        ExpireDate = DateTime.UtcNow.Add(lockTime);
    }

    public static byte[] Seriliaze(LockRequest message)
    {
        var keyBytes = Encoding.UTF8.GetBytes(message.Key);
        int totalLength = keyBytes.Length + _defaultLength;
        var result = new byte[keyBytes.Length + _defaultLength];
        var exprireDateBytes = BitConverter.GetBytes(message.ExpireDate.Ticks);
        var totalLengthBytes = BitConverter.GetBytes(totalLength);

        result[0] = totalLengthBytes[0];
        result[1] = totalLengthBytes[1];
        result[2] = totalLengthBytes[2];
        result[3] = totalLengthBytes[3];
        result[4] = exprireDateBytes[0];
        result[5] = exprireDateBytes[1];
        result[6] = exprireDateBytes[2];
        result[7] = exprireDateBytes[3];
        result[8] = exprireDateBytes[4];
        result[9] = exprireDateBytes[5];
        result[10] = exprireDateBytes[6];
        result[11] = exprireDateBytes[7];
        for (int i = 0; i < keyBytes.Length; i++)
            result[i + _defaultLength] = keyBytes[i];

        return result;
    }

    public static LockRequest Deseriliaze(byte[] data)
    {
        var ticks = BitConverter.ToInt64(data.AsSpan(0, 8));
        var key = Encoding.UTF8.GetString(data.AsSpan(_defaultLength).ToArray());
        return new LockRequest(key, new DateTime(ticks, DateTimeKind.Utc));
    }
}
