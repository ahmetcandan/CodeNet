namespace CodeNet.Elasticsearch.Settings;

public class ElasticsearchOptions
{
    public required string HostName { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
}

public class ElasticsearchOptions<TDbContext> : ElasticsearchOptions
    where TDbContext : ElasticsearchDbContext
{
}