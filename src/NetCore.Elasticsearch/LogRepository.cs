using Microsoft.Extensions.Options;
using NetCore.Abstraction;
using NetCore.Abstraction.Model;

namespace NetCore.Elasticsearch;

public class LogRepository(IOptions<ElasticsearchSettings> Config) : ElasticsearchRepository<LogModel>(Config), ILogRepository
{
    public Task<bool> AddAsync(LogModel model) => InsertAsync(model);
}
