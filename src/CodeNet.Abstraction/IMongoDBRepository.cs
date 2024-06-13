using CodeNet.Abstraction.Model;

namespace CodeNet.Abstraction;

public interface IMongoDBRepository<TModel> : IMongoDBSyncRepository<TModel>, IMongoDBAsyncRepository<TModel> where TModel : class, IBaseMongoDBModel, new()
{
}
