using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

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
