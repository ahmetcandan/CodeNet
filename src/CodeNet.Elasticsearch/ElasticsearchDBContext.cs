using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using Microsoft.Extensions.Options;
using CodeNet.Abstraction.Model;

namespace CodeNet.Elasticsearch;

public class ElasticsearchDBContext(IOptions<ElasticsearchSettings> config)
{
    public ElasticsearchClient Set()
    {
        var settings = new ElasticsearchClientSettings(new Uri(config.Value.HostName))
            .Authentication(new BasicAuthentication(config.Value.Username, config.Value.Password))
            .ServerCertificateValidationCallback((o, cer, chain, errors) => true);

        return new ElasticsearchClient(settings);
    }
}
