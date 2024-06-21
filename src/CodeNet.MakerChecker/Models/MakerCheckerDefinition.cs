using CodeNet.EntityFramework.Models;
using System.ComponentModel.DataAnnotations;

namespace CodeNet.MakerChecker.Models;

public class MakerCheckerDefinition : TracingEntity, ISoftDelete
{
    public required Guid Id { get; set; }

    [MaxLength(100)]
    public required string EntityName { get; set; }
}
