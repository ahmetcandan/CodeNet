using NetCore.Abstraction.Enum;
using System;

namespace NetCore.Abstraction.Model;

public class LogModel
{
    public virtual Guid Id { get; } = Guid.NewGuid();
    public virtual Guid RequestId { get; set; }
    public virtual DateTime LogDate { get; } = DateTime.Now;
    public virtual string Username { get; set; }
    public virtual string Namespace { get; set; }
    public virtual string ClassName { get; set; }
    public virtual string MethodName { get; set; }
    public virtual LogTime LogTime { get; set; }
    public virtual string Data { get; set; }
    public long Time { get; set; }
}
