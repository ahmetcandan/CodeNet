namespace CodeNet.Socket.Models;

public class Message(byte type, byte[] data)
{
    /// <summary>
    /// Type(1) + Length(4)
    /// </summary>
    private static readonly int _defaultLength = 5;
    public virtual byte Type { get; set; } = type;

    private byte[] _data = data;
    public virtual byte[] Data
    {
        get { return _data; }
        set
        {
            Length = value.Length;
            _data = value;
        }
    }
    public int Length { get; private set; } = data.Length;

    public byte[] Seriliaze()
    {
        var result = new byte[Data.Length + _defaultLength];

        result[0] = Type;

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
        return new(data[0], data[_defaultLength..]);
    }

    public static Message Deseriliaze(byte type, byte[] data)
    {
        return new(type, data);
    }
}
