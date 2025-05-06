namespace CodeNet.Socket.Models;

public enum MessageType : byte
{
    None = 0,
    Message = 1,
    Disconnected = 2,
    Validation = 3
}
