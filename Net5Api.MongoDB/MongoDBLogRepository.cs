using Microsoft.Extensions.Options;
using Net5Api.Abstraction;
using Net5Api.Abstraction.Model;

namespace Net5Api.MongoDB
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
