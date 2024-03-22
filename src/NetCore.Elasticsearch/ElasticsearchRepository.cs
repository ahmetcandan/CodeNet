using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using NetCore.Abstraction;
using NetCore.Abstraction.Model;

namespace NetCore.Elasticsearch;

public class ElasticsearchRepository : IElasticsearchRepository
{
    private readonly ElasticsearchClient _elasticsearchClient;
    private const string INDEX_NAME = "logs";

    public ElasticsearchRepository()
    {
        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
        };

        var settings = new ElasticsearchClientSettings(new Uri("https://localhost:9200"))
            .Authentication(new BasicAuthentication("elastic", "changeme"))
            .ServerCertificateValidationCallback((o, cer, chain, errors) => true);


        _elasticsearchClient = new ElasticsearchClient(settings);
    }

    public async Task SetData(LogModel log)
    {
        await _elasticsearchClient.IndexAsync(log, idx => idx.Index(INDEX_NAME));
    }
}
