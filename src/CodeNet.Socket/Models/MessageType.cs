namespace CodeNet.Socket.Models;

public enum MessageType : byte
{
    None = 0,
    Message = 1,
    Connected = 2,
    Disconnected = 3,
    Validation = 4
}
