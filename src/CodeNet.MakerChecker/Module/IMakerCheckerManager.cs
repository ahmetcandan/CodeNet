using CodeNet.MakerChecker.Models;

namespace CodeNet.MakerChecker;

public interface IMakerCheckerManager
{
    Guid InsertDefinition(DefinitionInserModel definition);
    Task<Guid> InsertDefinitionAsync(DefinitionInserModel definition, CancellationToken cancellationToken = default);
    DefinitionUpdateModel UpdateDefinition(DefinitionUpdateModel definition);
    Task<DefinitionUpdateModel> UpdateDefinitionAsync(DefinitionUpdateModel definition, CancellationToken cancellationToken = default);
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