using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using Microsoft.Extensions.Options;
using NetCore.Abstraction;
using NetCore.Abstraction.Model;

namespace NetCore.Elasticsearch;

public class ElasticsearchRepository<TModel> : IElasticsearchRepository<TModel> where TModel : class, IElasticsearchModel
{
    protected readonly ElasticsearchClient _elasticsearchClient;
    private readonly string _indexName;

    public ElasticsearchRepository(IOptions<ElasticsearchSettings> config, string indexName)
    {
        _indexName = indexName;
        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
        };

        var settings = new ElasticsearchClientSettings(new Uri(config.Value.HostName))
            .Authentication(new BasicAuthentication(config.Value.Username, config.Value.Password))
            .ServerCertificateValidationCallback((o, cer, chain, errors) => true);

        _elasticsearchClient = new ElasticsearchClient(settings);
    }

    public virtual Task<bool> InsertAsync(TModel model)
    {
        return InsertAsync(model, CancellationToken.None);
    }

    public virtual async Task<bool> InsertAsync(TModel model, CancellationToken cancellationToken)
    {
        var response = await _elasticsearchClient.IndexAsync(model, idx => idx.Index(_indexName), cancellationToken);
        return response.IsValidResponse;
    }

    public virtual Task<TModel?> GetAsync(Guid id)
    {
        return GetAsync(id, CancellationToken.None);
    }

    public virtual async Task<TModel?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        var response = await _elasticsearchClient.GetAsync<TModel>(id, idx => idx.Index(_indexName), cancellationToken);
        return response.IsValidResponse ? response.Source : null;
    }

    public virtual Task<bool> UpdateAsync(TModel model)
    {
        return UpdateAsync(model, CancellationToken.None);
    }

    public virtual async Task<bool> UpdateAsync(TModel model, CancellationToken cancellationToken)
    {
        var response = await _elasticsearchClient.UpdateAsync<TModel, TModel>(_indexName, model.Id, u => u.Doc(model), cancellationToken);
        return response.IsValidResponse;
    }

    public virtual Task<bool> DeleteAsync(Guid id)
    {
        return DeleteAsync(id, CancellationToken.None);
    }

    public virtual async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var response = await _elasticsearchClient.DeleteAsync(_indexName, id, cancellationToken);
        return response.IsValidResponse;
    }
}
