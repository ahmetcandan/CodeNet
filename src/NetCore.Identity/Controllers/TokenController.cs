using Microsoft.AspNetCore.Mvc;
using NetCore.Abstraction.Model;
using NetCore.Identity.Manager;
using NetCore.Identity.Model;

namespace NetCore.Identity.Controllers;

[Route("[controller]")]
[ApiController]
public class TokenController(IIdentityTokenManager IdentityTokenManager) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(200, Type = typeof(ResponseBase<TokenResponse>))]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        return Ok(await IdentityTokenManager.GenerateToken(model));
    }
}
