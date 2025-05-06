namespace CodeNet.EventBus.Models;

public enum MessageType : byte
{
    None = 0,
    Message = 1,
    Disconnected = 2,
    Validation = 3,
    SetClientType = 4,
    SetConsumerGroup = 5,
    SetChannel = 6,
    Publish = 7,
}
