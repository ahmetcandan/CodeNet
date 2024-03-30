﻿using MediatR;
using NetCore.Abstraction.Model;
using System.ComponentModel.DataAnnotations;

namespace NetCore.Identity.Model;

public class CreateRoleModel : IRequest<ResponseBase>
{
    public string Id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; }

    public string NormalizedName { get; set; }
}
