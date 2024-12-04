namespace CodeNet.EventBus.Models;

public class Message
{
    /// <summary>
    /// Type(1) + Length(4)
    /// </summary>
    private readonly static int _defaultLength = 5;
    public MessageType Type { get; set; }

    private byte[] _data;
    public virtual byte[] Data
    {
        get { return _data; }
        set
        {
            Length = value.Length;
            _data = value;
        }
    }
    public int Length { get; private set; }

    public byte[] Seriliaze()
    {
        var result = new byte[Data.Length + _defaultLength];

        result[0] = (byte)Type;

        var _length = BitConverter.GetBytes(Length);
        result[1] = _length[0];
        result[2] = _length[1];
        result[3] = _length[2];
        result[4] = _length[3];

        for (int i = 0; i < Data.Length; i++)
            result[i + _defaultLength] = Data[i];

        return result;
    }

    public static Message Deseriliaze(byte[] data)
    {
        return new()
        {
            Type = (MessageType)data[0],
            Data = data[_defaultLength..],
            Length = data.Length
        };
    }

    public static Message Deseriliaze(byte type, byte[] data)
    {
        return new()
        {
            Type = (MessageType)type,
            Data = data,
            Length = data.Length
        };
    }
}
