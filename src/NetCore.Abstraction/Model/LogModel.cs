using NetCore.Abstraction.Enum;
using System;

namespace NetCore.Abstraction.Model;

public class LogModel
{
    public virtual Guid Id { get; } = Guid.NewGuid();
    public virtual Guid RequestId { get; set; }
    public virtual DateTime LogDate { get; } = DateTime.Now;
    public virtual string? Username { get; set; }
    public virtual required string AssemblyName { get; set; }
    public virtual required string ClassName { get; set; }
    public virtual required string MethodName { get; set; }
    public virtual required string LogTime { get; set; }
    public virtual string? Data { get; set; }

    /// <summary>
    /// Total Miliseconds
    /// </summary>
    public virtual long? ElapsedDuration { get; set; }
}
