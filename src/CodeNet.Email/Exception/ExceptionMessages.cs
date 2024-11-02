using CodeNet.Core.Models;

namespace CodeNet.Email.Exception;

internal static class ExceptionMessages
{
    public static ExceptionMessage UseParams(this ExceptionMessage exceptionMessage, params string[] args) => new(exceptionMessage.Code, string.Format(exceptionMessage.Message, args));
}
