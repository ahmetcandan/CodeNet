namespace CodeNet.Identity.Models;

internal class RefreshTokenModel<TKey>
    where TKey : IEquatable<TKey>
{
    public required TKey UserId { get; set; }
    public required string RefreshToken { get; set; }
    public DateTime ExpiryTime { get; set; }
}
