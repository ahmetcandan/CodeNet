namespace CodeNet.Abstraction.Model;

public interface IBaseEntity : IEntity
{
    bool IsDeleted { get; set; }
    bool IsActive { get; set; }
}
