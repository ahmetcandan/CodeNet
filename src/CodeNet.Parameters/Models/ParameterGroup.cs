using CodeNet.EntityFramework.Models;

namespace CodeNet.Parameters.Models;

public class ParameterGroup : TracingEntity, ISoftDelete
{
    public virtual int Id { get; set; }
    public virtual string Code { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool ApprovalRequired { get; set; }
}
