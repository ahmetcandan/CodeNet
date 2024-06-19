using MediatR;
using CodeNet.Core.Models;

namespace CodeNet.Identity.Model;

public class LoginModel : IRequest<ResponseBase<TokenResponse>>
{
    //[Required(ErrorMessage = "User Name is required")]
    public string Username { get; set; }

    //[Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }
}
