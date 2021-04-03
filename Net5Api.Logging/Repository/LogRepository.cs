using Net5Api.MongoDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net5Api.Logging.Repository
{
    public class LogRepository : BaseMongoRepository<LogModel>, ILogRepository
    {
        public LogRepository() : base("Log")
        {

        }

        public void Insert(LogModel instance)
        {
            Create(instance);
        }
    }
}
