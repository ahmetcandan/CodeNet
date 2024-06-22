using CodeNet.EntityFramework.Models;

namespace CodeNet.MakerChecker.Models;

public interface IMakerCheckerEntity : ITracingEntity
{
    Guid ReferenceId { get; }
    ApproveStatus ApproveStatus { get; }
    void SetNewReferenceId();
    void SetApproveStatus(ApproveStatus approveStatus);
}