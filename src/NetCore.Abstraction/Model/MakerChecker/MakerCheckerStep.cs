namespace NetCore.Abstraction.Model;

public class MakerCheckerStep
{
    public int MakerCheckerId { get; set; }
    public MakerChecker MakerChecker { get; set; }
    public byte Step { get; set; }
    public string Username { get; set; }
    public Guid? UserGroupId { get; set; }
}
