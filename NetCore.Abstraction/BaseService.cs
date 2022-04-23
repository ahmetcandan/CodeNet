using System.Security.Principal;

namespace NetCore.Abstraction
{
    public abstract class BaseService
    {
        IPrincipal User;

        public virtual void SetUser(IPrincipal user)
        {
            User = user;
        }

        public virtual IPrincipal GetUser()
        {
            return User;
        }
    }
}
