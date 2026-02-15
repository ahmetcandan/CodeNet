using CodeNet.MakerChecker.Models;

namespace CodeNet.MakerChecker.Service;

public interface IMakerCheckerManager
{
    Guid InsertFlow<TMakerCheckerEntity>(FlowInserModel flow)
        where TMakerCheckerEntity : class, IMakerCheckerEntity;
    Guid InsertFlow(FlowInserModel flow, string entityName);
    Task<Guid> InsertFlowAsync<TMakerCheckerEntity>(FlowInserModel flow, CancellationToken cancellationToken = default)
        where TMakerCheckerEntity : class, IMakerCheckerEntity;
    Task<Guid> InsertFlowAsync(FlowInserModel flow, string entityName, CancellationToken cancellationToken = default);
    FlowUpdateModel UpdateFlow(FlowUpdateModel flow, string entityName);
    FlowUpdateModel UpdateFlow<TMakerCheckerEntity>(FlowUpdateModel flow)
        where TMakerCheckerEntity : class, IMakerCheckerEntity;
    Task<FlowUpdateModel> UpdateFlowAsync(FlowUpdateModel flow, string entityName, CancellationToken cancellationToken = default);
    Task<FlowUpdateModel> UpdateFlowAsync<TMakerCheckerEntity>(FlowUpdateModel flow, CancellationToken cancellationToken = default)
        where TMakerCheckerEntity : class, IMakerCheckerEntity;
    FlowUpdateModel DeleteFlow(Guid flowId);
    Task<FlowUpdateModel> DeleteFlowAsync(Guid flowId, CancellationToken cancellationToken = default);
    List<MakerCheckerPending> GetPendingList();
    Task<List<MakerCheckerPending>> GetPendingListAsync(CancellationToken cancellationToken = default);
    Task<TMakerCheckerEntity> ApproveAsync<TMakerCheckerEntity>(Guid referenceId, string description, CancellationToken cancellationToken = default)
        where TMakerCheckerEntity : class, IMakerCheckerEntity;
    TMakerCheckerEntity Approve<TMakerCheckerEntity>(Guid referenceId, string description)
        where TMakerCheckerEntity : class, IMakerCheckerEntity;
    Task<TMakerCheckerEntity> RejectAsync<TMakerCheckerEntity>(Guid referenceId, string description, CancellationToken cancellationToken = default)
        where TMakerCheckerEntity : class, IMakerCheckerEntity;
    TMakerCheckerEntity Reject<TMakerCheckerEntity>(Guid referenceId, string description)
        where TMakerCheckerEntity : class, IMakerCheckerEntity;
}