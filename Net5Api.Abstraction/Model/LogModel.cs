using Net5Api.Abstraction.Enum;
using System;
using System.Collections.Generic;

namespace Net5Api.Abstraction.Model
{
    public class LogModel : INoSqlModel
    {
        public LogModel()
        {
            LogDate = DateTime.Now;
        }

        public virtual string Id
        {
            get
            {
                return $"{UserName}:{LogDate.Ticks}";
            }
            set 
            {
                
            }
        }
        public virtual DateTime LogDate { get; set; }
        public virtual string UserName { get; set; }
        public virtual string Namespace { get; set; }
        public virtual string ClassName { get; set; }
        public virtual string MethodName { get; set; }
        public virtual LogTime LogTime { get; set; }
        public virtual LogType LogType { get; set; }
        public virtual string Message { get; set; }
        public virtual IEnumerable<MethodParameter> MethodParameters { get; set; }
    }
}
