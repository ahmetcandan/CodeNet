namespace CodeNet.EntityFramework.Models;

public interface IBaseEntity
{
    bool IsDeleted { get; set; }
    bool IsActive { get; set; }
}
