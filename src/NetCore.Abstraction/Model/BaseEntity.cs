namespace NetCore.Abstraction.Model
{
    public abstract class BaseEntity : Entity, IBaseEntity
    {
        protected BaseEntity()
        {
            IsActive = true;
            IsDeleted = false;
        }

        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
    }
}
