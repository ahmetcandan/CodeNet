using System.Security.Claims;

namespace Net5Api.Abstraction
{
    public interface IService
    {
        public void SetUser(ClaimsPrincipal user);

        public ClaimsPrincipal GetUser();
    }
}
