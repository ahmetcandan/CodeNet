using CodeNet.EntityFramework.Models;
using System.ComponentModel.DataAnnotations;

namespace CodeNet.MakerChecker.Models;

public class MakerCheckerFlow : TracingEntity, ISoftDelete
{
    public Guid Id { get; set; }

    public Guid DefinitionId { get; set; }

    public byte Order { get; set; }

    [MaxLength(250)]
    public required string Description { get; set; }

    [MaxLength(100)]
    public required string Approver { get; set; }

    public ApproveType ApproveType { get; set; }
}
