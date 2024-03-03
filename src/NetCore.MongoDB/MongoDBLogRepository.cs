using Microsoft.Extensions.Options;
using NetCore.Abstraction;
using NetCore.Abstraction.Model;

namespace NetCore.MongoDB
{
    public class MongoDBLogRepository : BaseMongoRepository<LogModel>, ILogRepository

    {
        public MongoDBLogRepository(IOptions<MongoDbSettings> config) : base(config.Value.ConnectionString, config.Value.DatabaseName, "Log")
        {

        }

        public void Insert(LogModel model)
        {
            Create(model);
        }
    }
}
