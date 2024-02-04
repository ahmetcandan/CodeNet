using System;
using System.Collections.Generic;

namespace NetCore.Identity.Model
{
    public class TokenResponse
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public DateTime CreatedDate { get; set; }
        public IEnumerable<ClaimResponse> Claims { get; set; }
    }
}
