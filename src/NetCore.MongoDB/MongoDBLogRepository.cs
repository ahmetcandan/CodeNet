using Microsoft.Extensions.Options;
using NetCore.Abstraction;
using NetCore.Abstraction.Model;

namespace NetCore.MongoDB;

public class MongoDBLogRepository(IOptions<MongoDbSettings> config) : BaseMongoRepository<LogModel>(config.Value.ConnectionString, config.Value.DatabaseName, "Log"), ILogRepository
{
    public void Insert(LogModel model)
    {
        Create(model);
    }
}
