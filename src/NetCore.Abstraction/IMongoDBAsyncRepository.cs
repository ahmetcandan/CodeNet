using System.Linq.Expressions;

namespace NetCore.Abstraction;

public interface IMongoDBAsyncRepository<TModel> where TModel : class, new()
{
    public Task<List<TModel>> GetListAsync(Expression<Func<TModel, bool>> filter);

    public Task<TModel> GetByIdAsync(Expression<Func<TModel, bool>> filter);

    public Task<TModel> CreateAsync(TModel model);

    public Task UpdateAsync(Expression<Func<TModel, bool>> filter, TModel model);

    public Task DeleteAsync(Expression<Func<TModel, bool>> filter);

    public Task<bool> ExistsAsync(Expression<Func<TModel, bool>> filter);

    public Task<long> CountAsync(Expression<Func<TModel, bool>> filter);
}
