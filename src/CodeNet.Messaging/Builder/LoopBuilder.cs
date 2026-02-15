using CodeNet.Messaging.Exception;
using System.Dynamic;
using System.Text;

namespace CodeNet.Messaging.Builder;

internal class LoopBuilder : IMessageBuilder
{
    private LoopBuilder() { }

    public static LoopBuilder Compile(string itemName, string arrayName, string rowBody, string content, int index)
    {
        LoopBuilder builder = new()
        {
            ItemName = itemName,
            Array = new(arrayName, ParamType.Parameter),
            RowContent = rowBody,
            Content = content,
            Index = index,
            BodyBuilder = MessageBuilder.Compile(rowBody)
        };

        return builder;
    }

    public StringBuilder Build(object? data)
    {
        Array?.SetValue(data);
        StringBuilder builder = new();
        if (Array?.HasValue is true)
        {
            dynamic dynamicObj = new ExpandoObject();
            var dictionary = (IDictionary<string, object>)dynamicObj;
            PopulateDictionary(data, dictionary, ItemName);
            foreach (var item in (dynamic)Array.Value!)
            {
                dictionary[ItemName] = item;
                builder.Append(BodyBuilder.Build(dynamicObj));
            }
        }

        return builder;
    }

    private static void PopulateDictionary(object? data, IDictionary<string, object> dictionary, string itemName)
    {
        if (data is not null)
            foreach (var prop in data.GetType().GetProperties())
            {
                if (prop.Name == itemName)
                    throw new MessagingException(ExceptionMessages.LoopItemParam);

                var value = prop.GetValue(data);
                if (value is not null)
                    dictionary[prop.Name] = value;
            }
    }

    public string Content { get; set; } = string.Empty;
    public int Index { get; set; }
    public ParamValue? Array { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public string RowContent { get; set; } = string.Empty;
    public required MessageBuilder BodyBuilder { get; set; }
    public BuildType Type { get; } = BuildType.Loop;
}
