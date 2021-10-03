using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Net5Api.Identity.Model
{
    public class RemoveUserModel
    {
        [Required(ErrorMessage = "User Name is required")]
        public string Username { get; set; }
    }
}
