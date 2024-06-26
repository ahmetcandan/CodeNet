namespace CodeNet.MakerChecker.Models;

internal class MakerCheckerFlowHistory
{
    public required MakerCheckerHistory MakerCheckerHistory { get; set; }
    public required string Approver { get; set; }
    public ApproveType ApproveType { get; set; }
    public byte Order { get; set; }
    public required string EntityName { get; set; }
}
