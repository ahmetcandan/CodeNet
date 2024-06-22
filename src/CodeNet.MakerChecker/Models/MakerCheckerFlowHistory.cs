namespace CodeNet.MakerChecker.Models;

internal class MakerCheckerFlowHistory
{
    public required MakerCheckerFlow MakerCheckerFlow { get; set; }
    public MakerCheckerHistory? MakerCheckerHistory { get; set; }
}
