namespace NetCore.Abstraction
{
    public interface IQService
    {
        public bool Post(string channelName, object data);
    }
}
