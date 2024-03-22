using System.Linq.Expressions;

namespace NetCore.Abstraction;

public interface INoSqlAsyncRepository<TModel>
{
    public Task<List<TModel>> GetListAsync();

    public Task<List<TModel>> GetListAsync(Expression<Func<TModel, bool>> filter);

    public Task<TModel> GetByIdAsync(string id);

    public Task<TModel> CreateAsync(TModel model);

    public Task UpdateAsync(string id, TModel model);

    public Task DeleteAsync(TModel model);

    public Task DeleteAsync(string id);

    public Task<bool> ContainsIdAsync(string id);
}
