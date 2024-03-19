using System;
using System.Collections.Generic;

namespace NetCore.Abstraction;

public interface IIdentityContext
{
    Guid RequestId { get; }
    string UserName { get; }
    string Email { get; }
    string UserId { get; }
    IEnumerable<string> Roles { get; }
    string Token { get; }
}
