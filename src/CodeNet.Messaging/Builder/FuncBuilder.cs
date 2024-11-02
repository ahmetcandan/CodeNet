using CodeNet.Messaging.Manager;
using System.Text;

namespace CodeNet.Messaging.Builder;

public class FuncBuilder : ITemplateBuilder
{
    FunctionExecuter _functionExecuter;

    private FuncBuilder()
    {
    }

    public static FuncBuilder Compile(string functionName, string content, int index)
    {
        FuncBuilder builder = new()
        {
            Content = content,
            FunctionName = functionName,
            Index = index
        };

        return builder;
    }

    public StringBuilder Build(object data)
    {
        _functionExecuter = new();
        foreach (var param in Parameters.Where(c => c.Type is ParamType.Parameter))
            param.SetValue(data);
        var method = typeof(FunctionExecuter).GetType().GetMethod(FunctionName, Parameters.Where(c => c.HasValue).Select(c => c.Value!.GetType()).ToArray());

        var result = method?.Invoke(_functionExecuter, Parameters.Select(c => c.Value).ToArray()) as string ?? string.Empty;
        return new(result);
    }

    public string Content { get; set; } = string.Empty;
    public int Index { get; set; }
    public string FunctionName { get; set; } = string.Empty;
    public ICollection<ParamValue> Parameters { get; set; } = [];
    public BuildType Type { get; } = BuildType.Func;
}
