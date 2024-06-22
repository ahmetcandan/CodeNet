using CodeNet.EntityFramework.Models;

namespace CodeNet.MakerChecker.Models;

public abstract class MakerCheckerEntity : TracingEntity, IMakerCheckerEntity
{
    private Guid _referenceId;
    private ApproveStatus _approveStatus;

    public Guid ReferenceId { get { return _referenceId; } }
    public ApproveStatus ApproveStatus { get { return _approveStatus; } }

    public void SetNewReferenceId()
    {
        _referenceId = Guid.NewGuid(); 
    }

    public void SetApproveStatus(ApproveStatus approveStatus)
    {
        _approveStatus = approveStatus;
    }
}
