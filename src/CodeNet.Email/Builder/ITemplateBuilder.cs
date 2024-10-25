using System.Text;

namespace CodeNet.Email.Builder;

internal interface ITemplateBuilder
{
    StringBuilder Build(object data);
    int Index { get; set; }
    string Content { get; set; }
    BuildType Type { get; }
}
