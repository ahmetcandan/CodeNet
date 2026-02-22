using System.Text;

namespace CodeNet.Messaging.Builder;

public interface IMessageBuilder
{
    StringBuilder Build(object? data);
    int Index { get; }
    string Content { get; }
}
