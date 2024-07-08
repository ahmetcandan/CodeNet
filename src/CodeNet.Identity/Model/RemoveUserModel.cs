using System.ComponentModel.DataAnnotations;

namespace CodeNet.Identity.Settings;

public class RemoveUserModel
{
    [Required(ErrorMessage = "User Name is required")]
    public string Username { get; set; }
}
