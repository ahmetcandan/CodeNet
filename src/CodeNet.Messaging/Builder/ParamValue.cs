namespace CodeNet.Messaging.Builder;

public class ParamValue
{
    public ParamValue()
    {
    }

    public ParamValue(string name, object value)
    {
        Name = name;
        Type = ParamType.StaticValue;
        Value = value;
    }

    public ParamValue(string name, ParamType type)
    {
        Name = name;
        Type = type;
    }

    public string Name { get; set; } = string.Empty;
    public object? Value { get; set; }
    public ParamType Type { get; set; }
    public bool HasValue { get { return Value is not null; } }

    public void SetValue(object obj)
    {
        switch (Type)
        {
            case ParamType.Parameter:
                Value = obj.GetValue(Name);
                break;
            default:
                break;
        }
    }
}
