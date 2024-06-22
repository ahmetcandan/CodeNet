using MediatR;
using CodeNet.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace CodeNet.Identity.Model;

public class RemoveUserModel : IRequest<ResponseBase>
{
    [Required(ErrorMessage = "User Name is required")]
    public string Username { get; set; }
}
