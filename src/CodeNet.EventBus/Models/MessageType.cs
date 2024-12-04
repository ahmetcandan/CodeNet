namespace CodeNet.EventBus.Models;

public enum MessageType : byte
{
    None = 0,
    Message = 1,
    SetClientType = 2,
    SetConsumerGroup = 3,
    SetChannel = 4,
    Publish = 5
}
