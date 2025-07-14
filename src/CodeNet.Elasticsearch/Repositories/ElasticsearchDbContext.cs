using CodeNet.Elasticsearch.Settings;
using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.Options;

namespace CodeNet.Elasticsearch;

public class ElasticsearchDbContext(IOptions<ElasticsearchOptions> config)
{
    public ElasticsearchClient Set() => new ElasticsearchClient(config.Value.ElasticsearchClientSettings);

    public async Task<bool> CanConnectionAsync(CancellationToken cancellationToken)
    {
        try
        {
            Id id = new(1);
            var result = await Set().GetScriptAsync(new GetScriptRequest(id), cancellationToken);
            return result?.IsSuccess() ?? false;
        }
        catch
        {
            return false;
        }
    }
}
