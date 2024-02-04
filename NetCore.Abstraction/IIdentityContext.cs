using System.Collections.Generic;

namespace NetCore.Abstraction
{
    public interface IIdentityContext
    {
        string GetUserName();
        IEnumerable<string> GetRoles();
        string GetToken();
    }
}