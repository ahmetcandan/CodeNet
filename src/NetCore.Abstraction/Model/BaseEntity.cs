namespace NetCore.Abstraction.Model
{
    public abstract class BaseEntity : IEntity
    {
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
    }
}
