using MediatR;
using CodeNet.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace CodeNet.Identity.Model;

public class GetUserQuery(string username) : IRequest<ResponseBase<UserModel>>
{
    [Required(ErrorMessage = "User Name is required")]
    public string Username { get; set; } = username;
}
