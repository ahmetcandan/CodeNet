﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Net5Api.Abstraction
{
    public interface IService
    {
        public void SetUser(ClaimsPrincipal user);

        public ClaimsPrincipal GetUser();
    }
}
