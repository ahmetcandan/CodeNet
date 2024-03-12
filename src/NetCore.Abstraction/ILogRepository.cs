using NetCore.Abstraction.Model;

namespace NetCore.Abstraction;

public interface ILogRepository
{
    public void Insert(LogModel model);
}
