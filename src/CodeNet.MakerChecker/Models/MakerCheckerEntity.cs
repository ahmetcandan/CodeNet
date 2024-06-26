using CodeNet.EntityFramework.Models;

namespace CodeNet.MakerChecker.Models;

public abstract class MakerCheckerEntity : TracingEntity, IMakerCheckerEntity
{
    public Guid ReferenceId { get; set; }

    public MakerCheckerDraftEntity NewDraft(EntryState entryState, byte order)
    {
        var draft = new MakerCheckerDraftEntity();
        draft.SetOrder(order);
        draft.SetNewReferenceId();
        draft.SetApproveStatus(ApproveStatus.Pending);
        draft.SetEntryState(entryState);
        return draft;
    }
}
