using System.Security.Principal;

namespace NetCore.Abstraction
{
    public abstract class BaseService
    {
        IPrincipal User;

        public void SetUser(IPrincipal user)
        {
            User = user;
        }

        public IPrincipal GetUser()
        {
            return User;
        }
    }
}
