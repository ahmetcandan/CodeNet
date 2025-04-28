namespace CodeNet.EventBus.Models;

public enum MessageType : byte
{
    None = 0,
    Message = 1,
    Disconnected = 2,
    SetClientType = 3,
    SetConsumerGroup = 4,
    SetChannel = 5,
    Publish = 6,
}
