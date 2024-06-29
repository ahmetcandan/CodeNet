namespace CodeNet.ExceptionHandling;

public class CodeNetException : Exception
{
    public CodeNetException(string code, string message)
    {
        Code = code;
        UserMessage = message;
    }

    public string UserMessage { get; set; }
    public string Code { get; set; }
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
