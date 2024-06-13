using CodeNet.Abstraction;
using CodeNet.Abstraction.Model;

namespace CodeNet.Elasticsearch;

public class LogRepository(ElasticsearchDBContext dbContext) : ElasticsearchRepository<LogModel>(dbContext), ILogRepository
{
    public Task<bool> AddAsync(LogModel model) => InsertAsync(model);
}
