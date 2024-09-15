using CodeNet.EntityFramework.Models;

namespace CodeNet.MakerChecker.Models;

public interface IMakerCheckerEntity : ITracingEntity
{
    Guid ReferenceId { get; set; }
    Guid? MainReferenceId { get; set; }
    EntryState EntryState { get; set; }
    EntityStatus EntityStatus { get; set; }
    byte Order { get; set; }
}