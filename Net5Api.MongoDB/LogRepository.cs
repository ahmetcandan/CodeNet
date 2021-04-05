using Net5Api.Abstraction;
using Net5Api.Abstraction.Model;

namespace Net5Api.MongoDB
{
    public class LogRepository : BaseMongoRepository<LogModel>, ILogRepository

    {
        public LogRepository() : base("Log")
        {

        }

        public void Insert(LogModel model)
        {
            Create(model);
        }
    }
}
