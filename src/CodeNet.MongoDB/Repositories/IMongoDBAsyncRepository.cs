using System.Linq.Expressions;

namespace CodeNet.MongoDB.Repositories;

public interface IMongoDBAsyncRepository<TModel> where TModel : class
{
    Task<List<TModel>> GetListAsync(Expression<Func<TModel, bool>> filter);
    Task<List<TModel>> GetListAsync(Expression<Func<TModel, bool>> filter, CancellationToken cancellationToken);
    Task<List<TModel>> GetPagingListAsync(Expression<Func<TModel, bool>> filter, Expression<Func<TModel, object>> orderBySelector, bool isAcending, int page, int count, CancellationToken cancellationToken);

    Task<TModel?> GetByIdAsync(Expression<Func<TModel, bool>> filter);

    Task<TModel?> GetByIdAsync(Expression<Func<TModel, bool>> filter, CancellationToken cancellationToken);

    Task<TModel> CreateAsync(TModel model);
    Task<TModel> CreateAsync(TModel model, CancellationToken cancellationToken);
    Task<IEnumerable<TModel>> CreateAsync(IEnumerable<TModel> models);
    Task<IEnumerable<TModel>> CreateAsync(IEnumerable<TModel> models, CancellationToken cancellationToken);

    Task UpdateAsync(Expression<Func<TModel, bool>> filter, TModel model);
    Task UpdateAsync(Expression<Func<TModel, bool>> filter, TModel model, CancellationToken cancellationToken);
    Task UpdateAsync(Expression<Func<TModel, bool>> filter, IList<KeyValuePair<string, object>> updatedValues, CancellationToken cancellationToken);

    Task DeleteAsync(Expression<Func<TModel, bool>> filter);

    Task DeleteAsync(Expression<Func<TModel, bool>> filter, CancellationToken cancellationToken);

    Task<bool> ExistsAsync(Expression<Func<TModel, bool>> filter);

    Task<bool> ExistsAsync(Expression<Func<TModel, bool>> filter, CancellationToken cancellationToken);

    Task<long> CountAsync(Expression<Func<TModel, bool>> filter);

    Task<long> CountAsync(Expression<Func<TModel, bool>> filter, CancellationToken cancellationToken);
}
