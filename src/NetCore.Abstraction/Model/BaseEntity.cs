namespace NetCore.Abstraction.Model;

public abstract class BaseEntity : Entity, IBaseEntity
{
    public bool IsDeleted { get; set; } = false;
    public bool IsActive { get; set; } = true;
}
