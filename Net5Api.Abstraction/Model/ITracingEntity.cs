using System;
using System.Collections.Generic;
using System.Text;

namespace Net5Api.Abstraction.Model
{
    public interface ITracingEntity : IEntity
    {
        string CreatedUser { get; set; }
        DateTime CreatedDate { get; set; }
        string ModifiedUser { get; set; }
        DateTime? ModifiedDate { get; set; }
        bool IsDeleted { get; set; }
        bool IsActive { get; set; }
    }
}
