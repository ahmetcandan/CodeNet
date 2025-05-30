﻿using System.ComponentModel.DataAnnotations;

namespace CodeNet.Identity.Settings;

public class RegisterUserModel
{
    [Required(ErrorMessage = "User Name is required")]
    public string Username { get; set; }

    [EmailAddress]
    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }

    public IList<string> Roles { get; set; }
}
