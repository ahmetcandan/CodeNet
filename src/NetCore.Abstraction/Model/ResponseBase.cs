namespace NetCore.Abstraction.Model;

public class ResponseBase
{
    public ResponseBase()
    {
    }

    public ResponseBase(string messageCode, string message)
    {
        MessageCode = messageCode;
        Message = message;
    }

    public ResponseBase(bool isSuccessfull, string messageCode, string message)
    {
        IsSuccessfull = isSuccessfull;
        MessageCode = messageCode;
        Message = message;
    }

    public bool IsSuccessfull { get; set; }
    public string MessageCode { get; set; }
    public string Message { get; set; }
    public bool FromCache { get; set; } = false;
}

public class ResponseBase<T> : ResponseBase where T : class
{
    public ResponseBase()
    {
    }

    public ResponseBase(T? data, bool isSuccessfull = true)
    {
        IsSuccessfull = isSuccessfull;
        Data = data;
    }

    public ResponseBase(string messageCode, string message)
    {
        MessageCode = messageCode;
        Message = message;
    }

    public ResponseBase(bool isSuccessfull, string messageCode, string message)
    {
        IsSuccessfull = isSuccessfull;
        MessageCode = messageCode;
        Message = message;
    }

    public T? Data { get; set; }
}
