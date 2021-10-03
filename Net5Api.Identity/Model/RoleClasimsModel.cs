using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Net5Api.Identity.Model
{
    public class RoleClaimsModel
    {

        [Required(ErrorMessage = "Id is required")]
        public string Id { get; set; }

        public IList<Claim>  Claims { get; set; }
    }
}
