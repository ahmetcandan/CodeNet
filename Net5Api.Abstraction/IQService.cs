using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net5Api.Abstraction
{
    public interface IQService
    {
        public bool Post(string channelName, object data);
    }
}
