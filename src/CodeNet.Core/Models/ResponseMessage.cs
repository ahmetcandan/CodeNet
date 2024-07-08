namespace CodeNet.Core.Models;

public class ResponseMessage
{
    public ResponseMessage()
    {
    }

    public ResponseMessage(string messageCode, string message)
    {
        MessageCode = messageCode;
        Message = message;
    }

    public string? MessageCode { get; set; }
    public string? Message { get; set; }
}
