using Elastic.Clients.Elasticsearch;

namespace CodeNet.Elasticsearch.Settings;

public class ElasticsearchOptions
{
    public required ElasticsearchClientSettings ElasticsearchClientSettings { get; set; }
}

public class ElasticsearchOptions<TDbContext> : ElasticsearchOptions
    where TDbContext : ElasticsearchDbContext
{
}