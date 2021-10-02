using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Net5Api.Model
{
    public class RemoveModel
    {
        [Required(ErrorMessage = "User Name is required")]
        public string Username { get; set; }
    }
}
