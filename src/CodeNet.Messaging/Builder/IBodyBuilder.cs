using System.Text;

namespace CodeNet.Messaging.Builder;

public interface IBodyBuilder
{
    StringBuilder Build(object? data);
    int Index { get; set; }
    string Content { get; set; }
    BuildType Type { get; }
}
