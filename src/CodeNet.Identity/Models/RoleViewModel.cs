namespace CodeNet.Identity.Models;

public class RoleViewModel
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public string? NormalizedName { get; set; }
    public IEnumerable<ClaimModel>? Claims { get; set; }
}
