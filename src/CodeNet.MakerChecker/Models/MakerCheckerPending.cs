namespace CodeNet.MakerChecker.Models;

public class MakerCheckerPending
{
    public Guid ReferenceId { get; set; }
    public required MakerCheckerHistory History { get; set; }
    public required MakerCheckerFlow Flow { get; set; }
}
