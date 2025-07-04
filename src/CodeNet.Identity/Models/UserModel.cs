namespace CodeNet.Identity.Models;

public class UserModel
{
    public required string Username { get; set; }

    public IList<string>? Roles { get; set; }
    public required string Id { get; set; }
    public required string Email { get; set; }
    public IEnumerable<ClaimModel>? Claims { get; set; }
}
