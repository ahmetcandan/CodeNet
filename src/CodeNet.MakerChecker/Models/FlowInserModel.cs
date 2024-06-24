
namespace CodeNet.MakerChecker.Models;

public class FlowInserModel
{
    public required string Approver { get; set; }
    public required string Description { get; set; }
    public ApproveType ApproveType { get; set; }
    public byte Order { get; set; }
    public Guid DefinitionId { get; set; }
}
