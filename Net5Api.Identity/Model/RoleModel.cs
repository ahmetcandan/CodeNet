using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Net5Api.Identity.Model
{
    public class RoleModel
    {

        [Required(ErrorMessage = "Id is required")]
        public string Id { get; set; }
        
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        public string NormalizedName { get; set; }
    }
}
