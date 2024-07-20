namespace CodeNet.MongoDB.Repositories;

public interface IMongoDBRepository<TModel> : IMongoDBSyncRepository<TModel>, IMongoDBAsyncRepository<TModel> where TModel : class
{
}
