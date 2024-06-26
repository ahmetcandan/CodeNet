using CodeNet.EntityFramework.Models;

namespace CodeNet.MakerChecker.Models;

public interface IMakerCheckerEntity : ITracingEntity
{
    Guid ReferenceId { get; set; }
    MakerCheckerDraftEntity NewDraft(EntryState entryState, byte order);
}