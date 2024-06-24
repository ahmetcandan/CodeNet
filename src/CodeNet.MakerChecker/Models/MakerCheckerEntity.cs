using CodeNet.EntityFramework.Models;

namespace CodeNet.MakerChecker.Models;

public abstract class MakerCheckerEntity : TracingEntity, IMakerCheckerEntity
{
    private Guid _referenceId;
    private ApproveStatus _approveStatus;

    public Guid ReferenceId { get { return _referenceId; } private set { _referenceId = value; } }
    public ApproveStatus ApproveStatus { get { return _approveStatus; } private set { _approveStatus = value; } }

    public void SetNewReferenceId()
    {
        _referenceId = Guid.NewGuid(); 
    }

    public void SetApproveStatus(ApproveStatus approveStatus)
    {
        _approveStatus = approveStatus;
    }
}
