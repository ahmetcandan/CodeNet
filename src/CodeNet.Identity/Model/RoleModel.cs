﻿using System.ComponentModel.DataAnnotations;

namespace CodeNet.Identity.Settings;

public class RoleModel
{
    [Required(ErrorMessage = "Id is required")]
    public string Id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; }

    public string NormalizedName { get; set; }
}
