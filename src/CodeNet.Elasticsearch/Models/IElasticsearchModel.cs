namespace CodeNet.Elasticsearch.Models;

public interface IElasticsearchModel
{
    Guid Id { get; }
    DateTime Date { get; }
}
