using CodeNet.Abstraction.Model;
using System.Linq.Expressions;

namespace CodeNet.Abstraction;

public interface IMongoDBAsyncRepository<TModel> where TModel : class, IBaseMongoDBModel, new()
{
    public Task<List<TModel>> GetListAsync(Expression<Func<TModel, bool>> filter);
    public Task<List<TModel>> GetListAsync(Expression<Func<TModel, bool>> filter, CancellationToken cancellationToken);

    public Task<TModel> GetByIdAsync(Expression<Func<TModel, bool>> filter);

    public Task<TModel> GetByIdAsync(Expression<Func<TModel, bool>> filter, CancellationToken cancellationToken);

    public Task<TModel> CreateAsync(TModel model);

    public Task<TModel> CreateAsync(TModel model, CancellationToken cancellationToken);

    public Task UpdateAsync(Expression<Func<TModel, bool>> filter, TModel model);

    public Task UpdateAsync(Expression<Func<TModel, bool>> filter, TModel model, CancellationToken cancellationToken);

    public Task DeleteAsync(Expression<Func<TModel, bool>> filter);

    public Task DeleteAsync(Expression<Func<TModel, bool>> filter, CancellationToken cancellationToken);

    public Task<bool> ExistsAsync(Expression<Func<TModel, bool>> filter);

    public Task<bool> ExistsAsync(Expression<Func<TModel, bool>> filter, CancellationToken cancellationToken);

    public Task<long> CountAsync(Expression<Func<TModel, bool>> filter);

    public Task<long> CountAsync(Expression<Func<TModel, bool>> filter, CancellationToken cancellationToken);
}
