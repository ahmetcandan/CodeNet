namespace CodeNet.Transport.Models;

public enum MessageType : byte
{
    None = 0,
    Message = 1,
    Disconnected = 2,
    Connected = 3,
    SetClientName = 4,
    UseSecutity = 5,
    SharePublicKey = 6,
    ShareAESKey = 7,
    ClienList = 8,
}
