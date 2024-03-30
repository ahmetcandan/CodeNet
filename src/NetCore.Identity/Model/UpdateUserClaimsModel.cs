﻿using MediatR;
using NetCore.Abstraction.Model;
using System.ComponentModel.DataAnnotations;

namespace NetCore.Identity.Model;

public class UpdateUserClaimsModel : IRequest<ResponseBase>
{
    [Required(ErrorMessage = "User Name is required")]
    public string Username { get; set; }

    public IEnumerable<EditClaimsModel> Claims { get; set; }
}
