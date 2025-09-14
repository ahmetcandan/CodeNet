using System.Linq.Expressions;

namespace CodeNet.MongoDB.Repositories;

public interface IMongoDBRepository<TModel> : IMongoDBSyncRepository<TModel>, IMongoDBAsyncRepository<TModel> where TModel : class
{
    TModel Create(TModel model);
    Task UpdateAsync(Expression<Func<TModel, bool>> filter, IList<KeyValuePair<string, object>> updatedValues, CancellationToken cancellationToken);
}
