using CodeNet.Elasticsearch;
using CodeNet.Elasticsearch.Repositories;
using StokTakip.Customer.Contract.Model;

namespace StokTakip.Customer.Repository;

public class TestElasticRepository : ElasticsearchRepository<ElasticModel>
{
    public TestElasticRepository(ElasticsearchDbContext dbContext) : base(dbContext)
    {
    }
}
