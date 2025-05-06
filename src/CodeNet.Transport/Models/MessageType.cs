namespace CodeNet.Transport.Models;

public enum MessageType : byte
{
    None = 0,
    Message = 1,
    Disconnected = 2,
    Validation = 3,
    Connected = 4,
    SetClientName = 5,
    UseSecutity = 6,
    SharePublicKey = 7,
    ShareAESKey = 8,
    ClienList = 9,
}
