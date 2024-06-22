﻿using MediatR;
using CodeNet.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace CodeNet.Identity.Model;

public class RoleClaimsModel : IRequest<ResponseBase>
{
    [Required(ErrorMessage = "Id is required")]
    public string Id { get; set; }

    public IEnumerable<EditClaimsModel> Claims { get; set; }
}
