using System.Security.Principal;

namespace Net5Api.Abstraction
{
    public interface IService
    {
        public void SetUser(IPrincipal user);

        public IPrincipal GetUser();
    }
}
