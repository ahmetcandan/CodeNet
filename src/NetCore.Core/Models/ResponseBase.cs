namespace NetCore.Core.Models
{
    public class ResponseBase
    {
        public ResponseBase()
        {

        }

        public ResponseBase(int statusCode)
        {
            StatusCode = statusCode;
        }

        public ResponseBase(int statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }

        public bool Success { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }

        public static ResponseBase SetStatusCode(int statusCode) => new ResponseBase(statusCode);

        public static ResponseBase SetStatusCode(int statusCode, string message) => new ResponseBase(statusCode, message);
    }

    public class ResponseBase<T> : ResponseBase
    {
        public ResponseBase()
        {

        }

        public ResponseBase(T data)
        {
            Data = data;
            StatusCode = 200;
            Success = true;
        }

        public ResponseBase(int statusCode)
        {
            StatusCode = statusCode;
        }

        public ResponseBase(int statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }

        public T Data { get; set; }

        public static ResponseBase<T> SetData(T data) => new ResponseBase<T>(data);

        public static ResponseBase<T> SetStatusCode(int statusCode) => new ResponseBase<T>(statusCode);

        public static ResponseBase<T> SetStatusCode(int statusCode, string message) => new ResponseBase<T>(statusCode, message);
    }
}
