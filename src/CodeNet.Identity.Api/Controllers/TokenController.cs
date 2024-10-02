using CodeNet.Core;
using CodeNet.Core.Models;
using CodeNet.Identity.Manager;
using CodeNet.Identity.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeNet.Identity.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class TokenController(IIdentityTokenManager tokenManager, ICodeNetContext codeNetContext) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(200, Type = typeof(TokenResponse))]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        return Ok(await tokenManager.GenerateToken(model));
    }

    [AllowAnonymous]
    [HttpPost("loginByRefreshToken")]
    [ProducesResponseType(200, Type = typeof(TokenResponse))]
    public async Task<IActionResult> LoginByRefreshToken([FromBody] LoginModelByToken model)
    {
        return Ok(await tokenManager.GenerateToken(model.Token, model.RefreshToken));
    }

    [Authorize]
    [HttpDelete("removeRefreshToken")]
    [ProducesResponseType(200, Type = typeof(ResponseMessage))]
    public async Task<IActionResult> RemoveRefreshToken()
    {
        return Ok(await tokenManager.RemoveRefreshToken(codeNetContext.UserName));
    }
}
