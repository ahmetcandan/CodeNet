namespace CodeNet.Messaging.Builder;

public class ParamValue(string name, ParamType type)
{
    public ParamValue(string name) : this(name, null) { }

    public ParamValue(string name, object? value) : this(name, ParamType.StaticValue) => Value = value;

    public string Name { get; set; } = name;
    public object? Value { get; set; }
    public ParamType Type { get; set; } = type;
    public bool HasValue { get { return Value is not null; } }

    public void SetValue(object? obj)
    {
        switch (Type)
        {
            case ParamType.Parameter:
                Value = obj?.GetValue(Name);
                break;
        }
    }
}
