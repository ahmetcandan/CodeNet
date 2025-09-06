using CodeNet.Elasticsearch.Attributes;
using CodeNet.Elasticsearch.Models;
using Elastic.Clients.Elasticsearch;
using System.Linq.Expressions;

namespace CodeNet.Elasticsearch.Repositories;

public class ElasticsearchRepository<TModel>(ElasticsearchDbContext dbContext) : IElasticsearchRepository<TModel> where TModel : class, IElasticsearchModel
{
    protected readonly ElasticsearchClient _elasticsearchClient = dbContext.Set();

    private readonly string _indexName = (typeof(TModel).GetCustomAttributes(typeof(IndexNameAttribute), true).FirstOrDefault() is not IndexNameAttribute indexAttribute
                ? typeof(TModel).Name
                : indexAttribute.Name).ToLower();

    /// <summary>
    /// Insert Data
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public virtual Task<bool> InsertAsync(TModel model) => InsertAsync(model, CancellationToken.None);

    /// <summary>
    /// Insert Data
    /// </summary>
    /// <param name="model"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<bool> InsertAsync(TModel model, CancellationToken cancellationToken) => (await _elasticsearchClient.IndexAsync(model, idx => idx.Index(_indexName), cancellationToken)).IsValidResponse;

    /// <summary>
    /// Get Data
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public virtual Task<TModel?> GetAsync(Guid id) => GetAsync(id, CancellationToken.None);

    /// <summary>
    /// GetData
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<TModel?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        var response = await _elasticsearchClient.GetAsync<TModel>(id, idx => idx.Index(_indexName), cancellationToken);
        return response.IsValidResponse ? response.Source : null;
    }

    public virtual async Task<IEnumerable<TModel>> GetListAsync(Expression<Func<TModel, bool>> predicate, CancellationToken cancellationToken)
    {
        var result = await _elasticsearchClient.SearchAsync<TModel>(s =>
            s.Query(q =>
                q.Bool(b =>
                    b.Filter(f =>
                        f.Script(script =>
                            script.Script(_s =>
                                _s.Source("params.predicate(doc)")
                                    .Params(p => p.Add("predicate", predicate))))))), cancellationToken);

        return result.Documents;
    }

    /// <summary>
    /// Update Data
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public virtual Task<bool> UpdateAsync(TModel model) => UpdateAsync(model, CancellationToken.None);

    /// <summary>
    /// Update Data
    /// </summary>
    /// <param name="model"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<bool> UpdateAsync(TModel model, CancellationToken cancellationToken) => (await _elasticsearchClient.UpdateAsync<TModel, TModel>(_indexName, model.Id, u => u.Doc(model), cancellationToken)).IsValidResponse;

    /// <summary>
    /// Delete Data
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public virtual Task<bool> DeleteAsync(Guid id) => DeleteAsync(id, CancellationToken.None);

    /// <summary>
    /// Delete Data
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken) => (await _elasticsearchClient.DeleteAsync(_indexName, id, cancellationToken)).IsValidResponse;
}
