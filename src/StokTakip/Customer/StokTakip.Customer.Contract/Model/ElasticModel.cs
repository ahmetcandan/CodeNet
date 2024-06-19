using CodeNet.Elasticsearch.Attributes;
using CodeNet.Elasticsearch.Models;

namespace StokTakip.Customer.Contract.Model;

[IndexName("Test")]
public class ElasticModel : IElasticsearchModel
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
}
