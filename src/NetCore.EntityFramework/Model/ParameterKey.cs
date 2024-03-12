using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetCore.EntityFramework.Model;

public class ParameterKey
{
    public int ParameterKeyId { get; set; }
    [Required]
    [StringLength(255)]
    public string KeyName { get; set; }
    [Required]
    [StringLength(100)]
    public string KeyType { get; set; }
    public virtual ICollection<ParameterValue> ParameterValues { get; set; } = [];
}
