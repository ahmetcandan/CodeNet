using CodeNet.EntityFramework.Models;
using System.ComponentModel.DataAnnotations;

namespace CodeNet.MakerChecker.Models;

public class MakerCheckerHistory : TracingEntity, ISoftDelete
{
    public Guid Id { get; set; }

    public Guid ReferenceId { get; set; }

    public Guid MakerCheckerFlowId { get; set; }

    public ApproveStatus ApproveStatus { get; set; }

    [MaxLength(250)]
    public string? Description { get; set; }
}
