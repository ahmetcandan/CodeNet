using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Net5Api.Model
{
    public class UpdateUserClaimsModel
    {
        [Required(ErrorMessage = "User Name is required")]
        public string Username { get; set; }

        public IList<Claim> Claims { get; set; }
    }
}
