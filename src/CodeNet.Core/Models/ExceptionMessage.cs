namespace CodeNet.Core.Models;

public struct ExceptionMessage(string code, string message)
{
    public string Code { get; set; } = code;
    public string Message { get; set; } = message;
}
