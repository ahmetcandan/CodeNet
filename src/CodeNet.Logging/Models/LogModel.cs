namespace CodeNet.Logging.Models;

public class LogModel
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime Date { get; } = DateTime.Now;
    public virtual string CorrelationId { get; set; } = string.Empty;
    public virtual string? Username { get; set; }
    public virtual string AssemblyName { get; set; } = string.Empty;
    public virtual string ClassName { get; set; } = string.Empty;
    public virtual string MethodName { get; set; } = string.Empty;
    public virtual string LogTime { get; set; } = string.Empty;
    public virtual string? Data { get; set; }

    /// <summary>
    /// Total Miliseconds
    /// </summary>
    public virtual long? ElapsedDuration { get; set; }
}
