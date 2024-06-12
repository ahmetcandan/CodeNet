using NetCore.Abstraction;
using NetCore.Abstraction.Model;

namespace NetCore.Elasticsearch;

public class LogRepository(ElasticsearchDBContext dbContext) : ElasticsearchRepository<LogModel>(dbContext), ILogRepository
{
    public Task<bool> AddAsync(LogModel model) => InsertAsync(model);
}
