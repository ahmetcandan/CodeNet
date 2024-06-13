using CodeNet.Abstraction.Model;

namespace CodeNet.Abstraction;

public interface ILogRepository
{
    public Task<bool> AddAsync(LogModel model);
}
