using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetCore.Abstraction.Model;

public class MakerChecker
{
    public int MakerCheckerId { get; set; }
    [Required]
    [StringLength(50)]
    public string TableName { get; set; }
    [Required]
    [StringLength(150)]
    public string MakerCheckerName { get; set; }
    public virtual ICollection<MakerCheckerStep> MakerCheckerSteps { get; set; }
    public virtual ICollection<MakerCheckerHistory> MakerCheckerHistories { get; set; }
}
