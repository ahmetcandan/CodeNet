using ServiceStack;
using System;

namespace NetCore.ExceptionHandling;

public class UserLevelException : Exception
{
    public UserLevelException()
    {

    }

    public UserLevelException(string code, string message)
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
            throw new UserLevelException("400", $"{(string.IsNullOrEmpty(parameterName) ? TypeNamne(typeof(TObject)) : parameterName)} cannot be empty.");
    }

    private static string TypeNamne(Type type)
    {
        if (type.GenericTypeArguments.Length > 0)
            return type.GenericTypeArguments[0].Name;

        return type.Name;
    }
}
