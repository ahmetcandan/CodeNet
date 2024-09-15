using CodeNet.EntityFramework.Models;

namespace CodeNet.MakerChecker.Models;

public abstract class MakerCheckerEntity : TracingEntity, IMakerCheckerEntity
{
    public Guid ReferenceId { get; set; }
    public Guid? MainReferenceId { get; set; }
    public EntryState EntryState { get; set; }
    public EntityStatus EntityStatus { get; set; }
    public byte Order { get; set; }
}
