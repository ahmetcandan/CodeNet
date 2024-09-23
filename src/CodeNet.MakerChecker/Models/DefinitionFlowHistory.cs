namespace CodeNet.MakerChecker.Models;

public class DefinitionFlowHistory
{
    public required string EntityName { get; set; }
    public required MakerCheckerFlow Flow { get; set; }
    public required MakerCheckerHistory History { get; set; }
}
