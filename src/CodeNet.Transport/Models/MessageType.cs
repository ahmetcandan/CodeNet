namespace CodeNet.Transport.Models;

public enum MessageType : byte
{
    None = 0,
    Message = 1,
    Connected = 2,
    Disconnected = 3,
    Validation = 4,
    ServerConfirmation = 5,
    ClientConfirmation = 6,
    ShareAESKey = 7,
    ClienList = 8,
}
