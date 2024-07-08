using System.ComponentModel.DataAnnotations;

namespace CodeNet.Identity.Settings;

public class GetUserQuery(string username)
{
    [Required(ErrorMessage = "User Name is required")]
    public string Username { get; set; } = username;
}
