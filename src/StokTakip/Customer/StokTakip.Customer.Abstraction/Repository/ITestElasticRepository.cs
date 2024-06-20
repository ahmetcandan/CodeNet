using CodeNet.Elasticsearch.Repositories;
using StokTakip.Customer.Contract.Model;

namespace StokTakip.Customer.Abstraction.Repository;

public interface ITestElasticRepository : IElasticsearchRepository<ElasticModel>
{
}
