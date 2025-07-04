namespace CodeNet.Identity.Models;

public class UserRefreshToken<TKey>
    where TKey : IEquatable<TKey>
{
    public required TKey UserId { get; set; }
    public required string RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryDate { get; set; }
}
