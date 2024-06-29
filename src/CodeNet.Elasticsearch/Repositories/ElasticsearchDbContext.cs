using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using Microsoft.Extensions.Options;
using CodeNet.Elasticsearch.Settings;

namespace CodeNet.Elasticsearch;

public class ElasticsearchDbContext(IOptions<ElasticsearchSettings> config)
{
    public ElasticsearchClient Set()
    {
        var settings = new ElasticsearchClientSettings(new Uri(config.Value.HostName))
            .Authentication(new BasicAuthentication(config.Value.Username, config.Value.Password))
            .ServerCertificateValidationCallback((o, cer, chain, errors) => true);

        return new ElasticsearchClient(settings);
    }

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
