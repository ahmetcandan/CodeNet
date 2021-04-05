using System.Security.Claims;

namespace Net5Api.Abstraction
{
    public abstract class BaseService
    {
        ClaimsPrincipal User;

        public void SetUser(ClaimsPrincipal user)
        {
            User = user;
        }

        public ClaimsPrincipal GetUser()
        {
            return User;
        }
    }
}
