using CodeNet.Core;
using CodeNet.Core.Models;
using CodeNet.Identity.Exception;
using CodeNet.Identity.Model;
using CodeNet.Identity.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace CodeNet.Identity.Manager;

internal abstract class IdentityTokenManager(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ICodeNetContext codeNetContext, IOptions<BaseIdentitySettings> options) : IIdentityTokenManager
{
    public async Task<TokenResponse> GenerateToken(LoginModel model, bool generateRefreshToken = true)
    {
        var user = await userManager.FindByNameAsync(model.Username);
        if (user is not null && await userManager.CheckPasswordAsync(user, model.Password))
            return await GenerateToken(user, generateRefreshToken);

        throw new IdentityException(ExceptionMessages.IncorrectUserPass);
    }

    public async Task<TokenResponse> GenerateToken(string token, string refreshToken)
    {
        var jwtSecurityToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
        var username = jwtSecurityToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        if (string.IsNullOrEmpty(username))
            throw new IdentityException(ExceptionMessages.InvalidToken);

        var user = await userManager.FindByNameAsync(username);
        if (user is null)
            throw new IdentityException(ExceptionMessages.UserNotFound);
        else if (!(user.RefreshToken?.Equals(refreshToken, StringComparison.OrdinalIgnoreCase) ?? false))
            throw new IdentityException(ExceptionMessages.InvalidRefreshToken);
        else if (user.RefreshTokenExpiryDate < DateTime.Now)
            throw new IdentityException(ExceptionMessages.RefreshTokenExpired);

        return await GenerateToken(user, false);
    }

    private async Task<TokenResponse> GenerateToken(ApplicationUser user, bool generateRefreshToken = true)
    {
        var now = DateTime.Now;
        var userRoles = await userManager.GetRolesAsync(user);

        var claims = new List<Claim>
                    {
                        new(ClaimTypes.Name, user.UserName),
                        new(ClaimTypes.NameIdentifier, user.Id),
                        new(ClaimTypes.Email, user.Email),
                        new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    };

        claims.AddRange(await userManager.GetClaimsAsync(user));
        foreach (var roleName in userRoles)
        {
            claims.Add(new Claim(ClaimTypes.Role, roleName));
            var role = await roleManager.FindByNameAsync(roleName);
            claims.AddRange(await roleManager.GetClaimsAsync(role));
        }

        claims.Add(new Claim("LoginTime", now.ToString("O"), "DateTime[O]"));

        var token = GetJwtSecurityToken(now, claims);
        string? refreshToken = null;
        DateTime? refreshTokenExpiryDate = null;
        if (generateRefreshToken)
        {
            user.RefreshToken = GenerateRefreshToken();
            user.RefreshTokenExpiryDate = now.AddHours(options.Value.RefreshTokenExpiryTime);
            await userManager.UpdateAsync(user);
            refreshToken = user.RefreshToken;
            refreshTokenExpiryDate = user.RefreshTokenExpiryDate;
        }


        return new TokenResponse
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = token.ValidTo,
            RefreshToken = refreshToken,
            RefreshTokenExpiration = refreshTokenExpiryDate,
            CreatedDate = DateTime.UtcNow,
            Claims = from c in claims
                     select new ClaimResponse
                     {
                         Type = c.Type,
                         Value = c.Value,
                         ValueType = c.ValueType
                     }
        };
    }

    public async Task<ResponseMessage> RemoveRefreshToken()
    {
        if (string.IsNullOrEmpty(codeNetContext.UserName))
            throw new IdentityException(ExceptionMessages.UserNotFound);

        var user = await userManager.FindByNameAsync(codeNetContext.UserName) ?? throw new IdentityException(ExceptionMessages.UserNotFound);
        user.RefreshTokenExpiryDate = null;
        user.RefreshToken = null;
        var result = await userManager.UpdateAsync(user);

        if (!result.Succeeded) 
            throw new IdentityException(ExceptionMessages.RefreshTokenRemoveFailed);

        return new ResponseMessage("000", "Removed refresh token");
    }

    internal abstract JwtSecurityToken GetJwtSecurityToken(DateTime now, List<Claim> claims);

    public static string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

}
