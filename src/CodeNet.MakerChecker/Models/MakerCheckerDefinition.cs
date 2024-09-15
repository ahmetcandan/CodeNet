using CodeNet.EntityFramework.Models;
using System.ComponentModel.DataAnnotations;

namespace CodeNet.MakerChecker.Models;

public class MakerCheckerDefinition : TracingEntity, ISoftDelete
{
    public required Guid Id { get; set; }

    [MaxLength(100)]
    [Required]
    public string EntityName { get; set; } = "";

    [MaxLength(250)]
    public string Description { get; set; }
}
