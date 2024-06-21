using CodeNet.EntityFramework.Models;
using System.ComponentModel.DataAnnotations;

namespace CodeNet.MakerChecker.Models;

public class MakerCheckerFlow : TracingEntity, ISoftDelete
{
    public Guid Id { get; set; }

    public Guid MakerCheckerDefinitionId { get; set; }

    [MaxLength(100)]
    public required string Approver { get; set; }
}
