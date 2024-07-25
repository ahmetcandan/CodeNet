namespace CodeNet.EntityFramework.Models;

public abstract class BaseEntity : IBaseEntity
{
    public bool IsDeleted { get; set; } = false;
    public bool IsActive { get; set; } = true;
}
