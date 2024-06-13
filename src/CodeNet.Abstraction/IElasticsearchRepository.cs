namespace CodeNet.Abstraction;

public interface IElasticsearchRepository<TModel> where TModel : class, IElasticsearchModel
{
    Task<bool> InsertAsync(TModel model, CancellationToken cancellationToken);
    Task<bool> InsertAsync(TModel model);
    Task<TModel?> GetAsync(Guid id);
    Task<TModel?> GetAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(TModel model);
    Task<bool> UpdateAsync(TModel model, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
}
