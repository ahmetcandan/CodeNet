using CodeNet.MakerChecker.Models;

namespace CodeNet.MakerChecker;

public interface IMakerCheckerManager
{
    Guid InsertDefinition<TMakerCheckerEntity>() where TMakerCheckerEntity : class, IMakerCheckerEntity;
    Task<Guid> InsertDefinitionAsync<TMakerCheckerEntity>(CancellationToken cancellationToken = default) where TMakerCheckerEntity : class, IMakerCheckerEntity;
    DefinitionUpdateModel UpdateDefinition<TMakerCheckerEntity>(DefinitionUpdateModel definition) where TMakerCheckerEntity : class, IMakerCheckerEntity;
    Task<DefinitionUpdateModel> UpdateDefinitionAsync<TMakerCheckerEntity>(DefinitionUpdateModel definition, CancellationToken cancellationToken = default) where TMakerCheckerEntity : class, IMakerCheckerEntity;
    DefinitionUpdateModel DeleteDefinition(Guid definitionId);
    Task<DefinitionUpdateModel> DeleteDefinitionAsync(Guid definitionId, CancellationToken cancellationToken = default);
    Guid InsertFlow(FlowInserModel flow);
    Task<Guid> InsertFlowAsync(FlowInserModel flow, CancellationToken cancellationToken = default);
    FlowUpdateModel UpdateFlow(FlowUpdateModel flow);
    Task<FlowUpdateModel> UpdateFlowAsync(FlowUpdateModel flow, CancellationToken cancellationToken = default);
    FlowUpdateModel DeleteFlow(Guid flowId);
    Task<FlowUpdateModel> DeleteFlowAsync(Guid flowId, CancellationToken cancellationToken = default);
    List<MakerCheckerPending> GetPendingList();
    Task<List<MakerCheckerPending>> GetPendingListAsync(CancellationToken cancellationToken = default);
}