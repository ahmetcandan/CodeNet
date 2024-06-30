using CodeNet.Core.Enums;

namespace CodeNet.Core;

public interface IIdentityContext
{
    Guid CorrelationId { get; }
    string UserName { get; }
    string Email { get; }
    string UserId { get; }
    IEnumerable<string> Roles { get; }
    string Token { get; }
    CacheState CacheState { get; }
}
