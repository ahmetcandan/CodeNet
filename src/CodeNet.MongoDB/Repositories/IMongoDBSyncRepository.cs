using System.Linq.Expressions;

namespace CodeNet.MongoDB.Repositories;

public interface IMongoDBSyncRepository<TModel> where TModel : class
{
    List<TModel> GetList(Expression<Func<TModel, bool>> filter);

    TModel GetById(Expression<Func<TModel, bool>> filter);

    TModel Create(TModel model);
    IEnumerable<TModel> Create(IEnumerable<TModel> models);

    void Update(Expression<Func<TModel, bool>> filter, TModel model);
    void Update(Expression<Func<TModel, bool>> filter, IList<KeyValuePair<string, object>> updatedValues);

    void Delete(Expression<Func<TModel, bool>> filter);

    bool Exists(Expression<Func<TModel, bool>> filter);

    long Count(Expression<Func<TModel, bool>> filter);
}
