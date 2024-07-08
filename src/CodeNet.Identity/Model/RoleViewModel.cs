namespace CodeNet.Identity.Settings;

public class RoleViewModel
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string NormalizedName { get; set; }
    public IEnumerable<ClaimModel> Claims { get; set; }
}
