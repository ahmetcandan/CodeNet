namespace CodeNet.MakerChecker.Models;

public class DefinitionFlowHistory
{
    public required MakerCheckerDefinition Definition { get; set; }
    public required MakerCheckerFlow Flow { get; set; }
    public required MakerCheckerHistory History { get; set; }
}
