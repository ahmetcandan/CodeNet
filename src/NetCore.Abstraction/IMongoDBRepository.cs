using NetCore.Abstraction.Model;

namespace NetCore.Abstraction;

public interface IMongoDBRepository<TModel> : IMongoDBSyncRepository<TModel>, IMongoDBAsyncRepository<TModel> where TModel : class, IBaseMongoDBModel, new()
{
}
