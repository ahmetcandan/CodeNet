using System.Security.Principal;

namespace NetCore.Abstraction
{
    public interface IService
    {
        public void SetUser(IPrincipal user);

        public IPrincipal GetUser();
    }
}
