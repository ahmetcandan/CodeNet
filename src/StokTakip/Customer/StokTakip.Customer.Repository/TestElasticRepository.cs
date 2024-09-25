using CodeNet.Elasticsearch;
using CodeNet.Elasticsearch.Repositories;
using StokTakip.Customer.Abstraction.Repository;
using StokTakip.Customer.Contract.Model;

namespace StokTakip.Customer.Repository;

public class TestElasticRepository(ElasticsearchDbContext dbContext) : ElasticsearchRepository<ElasticModel>(dbContext), ITestElasticRepository
{
}
