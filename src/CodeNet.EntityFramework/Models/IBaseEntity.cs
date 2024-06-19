namespace CodeNet.EntityFramework.Models;

public interface IBaseEntity : IEntity
{
    bool IsDeleted { get; set; }
    bool IsActive { get; set; }
}
