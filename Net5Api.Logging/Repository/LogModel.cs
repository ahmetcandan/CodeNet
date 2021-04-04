using Net5Api.MongoDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net5Api.Logging.Repository
{
    public class LogModel : BaseMongoModel
    {
        public LogModel()
        {
            _logDate = DateTime.Now;
        }

        DateTime _logDate;
        public DateTime LogDate
        {
            get
            {
                return _logDate;
            }
            set
            {
                _logDate = value;
            }
        }
        public override string Id
        {
            get
            {
                return $"{UserName}:{LogDate.Ticks}";
            }
        }
        public string UserName { get; set; }
        public string Namespace { get; set; }
        public string ClassName { get; set; }
        public string MethodName { get; set; }
        public LogTime LogTime { get; set; }
        public LogType LogType { get; set; }
        public string Message { get; set; }
        public IEnumerable<MethodParameter> MethodParameters { get; set; }
    }

    public class MethodParameter
    {
        public string Name { get; set; }
        public object Value { get; set; }
    }

    public enum LogTime
    {
        Before,
        After,
        BeforeAndAfter,
        Exception
    }

    public enum LogType
    {
        Info,
        Warning,
        Error
    }
}
