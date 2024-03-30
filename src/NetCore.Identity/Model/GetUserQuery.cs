using MediatR;
using NetCore.Abstraction.Model;
using System.ComponentModel.DataAnnotations;

namespace NetCore.Identity.Model;

public class GetUserQuery(string username) : IRequest<ResponseBase<UserModel>>
{
    [Required(ErrorMessage = "User Name is required")]
    public string Username { get; set; } = username;
}
