namespace CodeNet.ExceptionHandling;

public class CodeNetException(string code, string message) : Exception
{
    public string UserMessage { get; set; } = message;
    public string Code { get; set; } = code;
    public override string StackTrace => string.Empty;
    public override string Message => UserMessage;

    public static void ThrowIfNull<TObject>(TObject obj, string parameterName = "")
    {
        if (obj is null)
            throw new CodeNetException("400", $"{(string.IsNullOrEmpty(parameterName) ? TypeName(typeof(TObject)) : parameterName)} cannot be empty.");
    }

    private static string TypeName(Type type)
    {
        return type.GenericTypeArguments.Length > 0 ? type.GenericTypeArguments[0].Name : type.Name;
    }
}
