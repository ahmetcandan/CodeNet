using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Net5Api.Model
{
    public class UpdateModel
    {
        [Required(ErrorMessage = "User Name is required")]
        public string Username { get; set; }

        public IList<string> Roles { get; set; }
    }
}
