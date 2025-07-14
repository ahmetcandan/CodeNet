using CodeNet.Core.Models;

namespace CodeNet.ExceptionHandling;

public class CodeNetException : Exception
{
    public int? HttpStatusCode { get; set; }
    public string UserMessage { get; set; }
    public string Code { get; set; }
    public override string StackTrace => string.Empty;
    public override string Message => UserMessage;

    public CodeNetException(string code, string message)
    {
        UserMessage = message;
        Code = code;
    }

    public CodeNetException(ExceptionMessage exceptionMessage)
    {
        UserMessage = exceptionMessage.Message;
        Code = exceptionMessage.Code;
    }

    public CodeNetException(string code, string message, int httpStatusCode)
    {
        UserMessage = message;
        Code = code;
        HttpStatusCode = httpStatusCode;
    }

    public static void ThrowIfNull<TObject>(TObject obj, string parameterName = "")
    {
        if (obj is null)
            throw new CodeNetException("400", $"{(string.IsNullOrEmpty(parameterName) ? TypeName(typeof(TObject)) : parameterName)} cannot be empty.");
    }

    private static string TypeName(Type type) => type.GenericTypeArguments.Length > 0 ? type.GenericTypeArguments[0].Name : type.Name;
}
