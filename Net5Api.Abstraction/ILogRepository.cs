using Net5Api.Abstraction.Model;

namespace Net5Api.Abstraction
{
    public interface ILogRepository
    {
        public void Insert(LogModel model);
    }
}
