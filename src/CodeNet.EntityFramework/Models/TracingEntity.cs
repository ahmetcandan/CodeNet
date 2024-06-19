using System.ComponentModel.DataAnnotations;

namespace CodeNet.EntityFramework.Models;

public abstract class TracingEntity : BaseEntity, ITracingEntity
{
    [MaxLength(100)]
    public string? CreatedUser { get; set; }

    [Required]
    public DateTime CreatedDate { get; set; }

    [MaxLength(100)]
    public string? ModifiedUser { get; set; }

    public DateTime? ModifiedDate { get; set; }
}
