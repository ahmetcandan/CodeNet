using System;
using System.ComponentModel.DataAnnotations;

namespace NetCore.Abstraction.Model;

public abstract class TracingEntity : BaseEntity, ITracingEntity
{
    [MaxLength(100)]
    public string CreatedUser { get; set; }

    [Required]
    public DateTime CreatedDate { get; set; }

    [MaxLength(100)]
    public string ModifiedUser { get; set; }

    public DateTime? ModifiedDate { get; set; }
}
