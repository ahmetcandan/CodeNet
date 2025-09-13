using System.Text;

namespace CodeNet.Messaging.Builder;

public interface ITemplateBuilder
{
    StringBuilder Build(object? data);
    int Index { get; set; }
    string Content { get; set; }
    BuildType Type { get; }
}
