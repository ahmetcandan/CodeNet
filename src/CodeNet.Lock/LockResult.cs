using System.Text;

namespace CodeNet.Lock;

public class LockResult
{
    private const int _defaultLength = 13;

    public bool IsSuccess { get; set; }
    public DateTime ExpireDate { get; set; }
    public string Message { get; set; } = string.Empty;

    public static byte[] Seriliaze(LockResult message)
    {
        var messageBytes = Encoding.UTF8.GetBytes(message.Message);
        int totalLength = messageBytes.Length + _defaultLength;
        var result = new byte[messageBytes.Length + _defaultLength];
        var exprireDateBytes = BitConverter.GetBytes(message.ExpireDate.Ticks);
        var totalLengthBytes = BitConverter.GetBytes(totalLength);
        result[0] = totalLengthBytes[0];
        result[1] = totalLengthBytes[1];
        result[2] = totalLengthBytes[2];
        result[3] = totalLengthBytes[3];
        result[4] = message.IsSuccess ? (byte)1 : (byte)0;
        result[5] = exprireDateBytes[0];
        result[6] = exprireDateBytes[1];
        result[7] = exprireDateBytes[2];
        result[8] = exprireDateBytes[3];
        result[9] = exprireDateBytes[4];
        result[10] = exprireDateBytes[5];
        result[11] = exprireDateBytes[6];
        result[12] = exprireDateBytes[7];

        for (int i = 0; i < messageBytes.Length; i++)
            result[i + _defaultLength] = messageBytes[i];

        return result;
    }

    public static LockResult Deseriliaze(byte[] data)
    {
        var isSuccess = data[0] == 1;
        var ticks = BitConverter.ToInt64(data.AsSpan(1, 8));
        var message = Encoding.UTF8.GetString(data.AsSpan(_defaultLength).ToArray());
        return new LockResult
        {
            IsSuccess = isSuccess,
            ExpireDate = new DateTime(ticks, DateTimeKind.Utc),
            Message = message
        };
    }
}
