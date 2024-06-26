using CodeNet.EntityFramework.Models;
using Newtonsoft.Json;
using System.Text;

namespace CodeNet.MakerChecker.Models;

public class MakerCheckerDraftEntity : TracingEntity
{
    private Guid _id;
    private ApproveStatus _approveStatus;
    private EntryState _entryState;
    private byte _order;
    private byte[] _entityData;

    public Guid Id { get { return _id; } private set { _id = value; } }
    public ApproveStatus ApproveStatus { get { return _approveStatus; } private set { _approveStatus = value; } }
    public EntryState EntryState { get { return _entryState; } private set { _entryState = value; } }
    public byte[] EntityData { get { return _entityData; } private set { _entityData = value; } }

    public byte Order { get { return _order; } private set { _order = value; } }

    public void SetNewReferenceId()
    {
        _id = Guid.NewGuid();
    }

    public void SetApproveStatus(ApproveStatus approveStatus)
    {
        _approveStatus = approveStatus;
    }

    public void SetEntryState(EntryState entryState)
    {
        _entryState = entryState;
    }

    public void SetOrder(byte order)
    {
        _order = order;
    }

    public virtual void SetEntity<TMakerCheckerEntity>(TMakerCheckerEntity entity)
        where TMakerCheckerEntity : class, IMakerCheckerEntity
    {
        ArgumentNullException.ThrowIfNull(entity);

        _entityData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(entity));
    }

    public virtual TMakerCheckerEntity? GetEntity<TMakerCheckerEntity>()
        where TMakerCheckerEntity : class, IMakerCheckerEntity
    {
        return JsonConvert.DeserializeObject<TMakerCheckerEntity>(Encoding.UTF8.GetString(_entityData));
    }
}
