using System;

namespace NetCore.Abstraction.Model
{
    public abstract class TracingEntity : BaseEntity, ITracingEntity
    {
        public string CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? ModifiedUser { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
