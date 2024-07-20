using CodeNet.EntityFramework.Models;
using Newtonsoft.Json;
using System.Text;

namespace CodeNet.MakerChecker.Models;

public class MakerCheckerDraftEntity : TracingEntity
{
    public Guid Id { get; private set; }
    public ApproveStatus ApproveStatus { get; private set; }
    public EntryState EntryState { get; private set; }
    public byte[] EntityData { get; private set; }

    public byte Order { get; private set; }

    public void SetNewReferenceId()
    {
        Id = Guid.NewGuid();
    }

    public void SetApproveStatus(ApproveStatus approveStatus)
    {
        ApproveStatus = approveStatus;
    }

    public void SetEntryState(EntryState entryState)
    {
        EntryState = entryState;
    }

    public void SetOrder(byte order)
    {
        Order = order;
    }

    public virtual void SetEntity<TMakerCheckerEntity>(TMakerCheckerEntity entity)
        where TMakerCheckerEntity : class, IMakerCheckerEntity
    {
        ArgumentNullException.ThrowIfNull(entity);

        EntityData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(entity));
    }

    public virtual TMakerCheckerEntity? GetEntity<TMakerCheckerEntity>()
        where TMakerCheckerEntity : class, IMakerCheckerEntity
    {
        return JsonConvert.DeserializeObject<TMakerCheckerEntity>(Encoding.UTF8.GetString(EntityData));
    }
}
