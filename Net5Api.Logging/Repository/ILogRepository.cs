using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net5Api.Logging.Repository
{
    public interface ILogRepository
    {
        void Insert(LogModel instance);
    }
}
