using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net5Api.ExceptionHandling
{
    public class UserLevelException : Exception
    {
        public string UserMessage { get; set; }
        public override string StackTrace => string.Empty;
        public override string Message => UserMessage;
    }
}
