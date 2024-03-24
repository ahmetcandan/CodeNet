using NetCore.Abstraction.Model;

namespace NetCore.Abstraction;

public interface ILogRepository
{
    public Task<bool> AddAsync(LogModel model);
}
