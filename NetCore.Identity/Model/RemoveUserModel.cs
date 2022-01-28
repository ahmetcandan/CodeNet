using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetCore.Identity.Model
{
    public class RemoveUserModel
    {
        [Required(ErrorMessage = "User Name is required")]
        public string Username { get; set; }
    }
}
