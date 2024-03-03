namespace NetCore.Abstraction.Model
{
    public class ResponseBase
    {
        public bool IsSuccessfull { get; set; }
        public string MessageCode { get; set; }
        public string Message { get; set; }
    }

    public class ResponseBase<T> : ResponseBase
    {
        public T Data { get; set; }
    }
}
