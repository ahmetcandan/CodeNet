using CodeNet.Core.Enums;
using Microsoft.Extensions.Primitives;

namespace CodeNet.Core;

public interface ICodeNetHttpContext
{
    Guid CorrelationId { get; }
    string UserName { get; }
    string Email { get; }
    string UserId { get; }
    IEnumerable<string> Roles { get; }
    string Token { get; }
    CacheState CacheState { get; }
    bool SetResponseHeader(string key, string value);
    StringValues GetResponseHeader(string key);
    StringValues GetRequestHeader(string key);
}
