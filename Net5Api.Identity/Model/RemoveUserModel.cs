using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Net5Api.Model
{
    public class RemoveUserModel
    {
        [Required(ErrorMessage = "User Name is required")]
        public string Username { get; set; }
    }
}
