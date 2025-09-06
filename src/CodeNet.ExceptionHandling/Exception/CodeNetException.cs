using CodeNet.Core.Models;

namespace CodeNet.ExceptionHandling;

public class CodeNetException(string code, string message) : Exception
{
    public int? HttpStatusCode { get; set; }
    public string UserMessage { get; set; } = message;
    public string Code { get; set; } = code;
    public override string StackTrace => string.Empty;
    public override string Message => UserMessage;

    public CodeNetException(ExceptionMessage exceptionMessage) : this(exceptionMessage.Code, exceptionMessage.Message) { }

    public CodeNetException(string code, string message, int httpStatusCode) : this (code, message) => HttpStatusCode = httpStatusCode;

    public static void ThrowIfNull<TObject>(TObject obj, string parameterName = "")
    {
        if (obj is null)
            throw new CodeNetException("400", $"{(string.IsNullOrEmpty(parameterName) ? TypeName(typeof(TObject)) : parameterName)} cannot be empty.");
    }

    private static string TypeName(Type type) => type.GenericTypeArguments.Length > 0 ? type.GenericTypeArguments[0].Name : type.Name;
}
