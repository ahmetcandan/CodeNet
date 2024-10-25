using System.Text;

namespace CodeNet.Email.Builder;

internal class LoopBuilder : ITemplateBuilder
{
    private LoopBuilder()
    {
    }

    public static LoopBuilder Compile(string itemName, string arrayName, string rowBody, string content, int index)
    {
        LoopBuilder builder = new()
        {
            ItemName = itemName,
            Array = new(arrayName, ParamType.Parameter),
            RowContent = rowBody,
            Content = content,
            Index = index,
            BodyBuilder = TemplateBuilder.Compile(rowBody)
        };

        return builder;
    }

    public StringBuilder Build(object data)
    {
        Array?.SetValue(data);
        StringBuilder builder = new();
        if (Array?.HasValue is true)
            foreach (var item in (dynamic)Array.Value!)
                builder.Append(BodyBuilder.Build(new { item }));

        return builder;
    }

    public string Content { get; set; } = string.Empty;
    public int Index { get; set; }
    public ParamValue? Array { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public string RowContent { get; set; } = string.Empty;
    public required TemplateBuilder BodyBuilder { get; set; }
    public BuildType Type { get; } = BuildType.Loop;
}
