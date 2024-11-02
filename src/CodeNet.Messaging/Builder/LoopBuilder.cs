using CodeNet.Messaging.Exception;
using System.Dynamic;
using System.Text;

namespace CodeNet.Messaging.Builder;

public class LoopBuilder : ITemplateBuilder
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
        {
            dynamic dynamicObj = new ExpandoObject();
            var dictionary = (IDictionary<string, object>)dynamicObj;
            var type = data.GetType();
            foreach (var prop in type.GetProperties())
            {
                if (prop.Name == ItemName)
                    throw new MessagingException(ExceptionMessages.LoopItemParam);

                var value = prop.GetValue(data);
                if (value is not null)
                    dictionary[prop.Name] = value;
            }
            foreach (var item in (dynamic)Array.Value!)
            {
                dictionary[ItemName] = item;
                builder.Append(BodyBuilder.Build(dynamicObj));
            }
        }

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
