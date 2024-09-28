using CodeNet.Core.Models;
using CodeNet.Identity.Manager;
using CodeNet.Identity.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeNet.Identity.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class TokenController(IIdentityTokenManager tokenManager) : ControllerBase
{
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(200, Type = typeof(TokenResponse))]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        return Ok(await tokenManager.GenerateToken(model));
    }

    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(200, Type = typeof(TokenResponse))]
    public async Task<IActionResult> Login(string token, string refreshToken)
    {
        return Ok(await tokenManager.GenerateToken(token, refreshToken));
    }

    [HttpDelete]
    [Authorize]
    [ProducesResponseType(200, Type = typeof(ResponseMessage))]
    public async Task<IActionResult> RemoveRefreshToken()
    {
        return Ok(await tokenManager.RemoveRefreshToken());
    }
}
