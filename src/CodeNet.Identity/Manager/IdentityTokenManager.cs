using CodeNet.Core.Models;
using CodeNet.Identity.Exception;
using CodeNet.Identity.Model;
using CodeNet.Identity.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace CodeNet.Identity.Manager;

internal abstract class IdentityTokenManager<TUser>(UserManager<TUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<BaseIdentitySettings> options) : IIdentityTokenManager
    where TUser : ApplicationUser
{
    public async Task<TokenResponse> GenerateToken(LoginModel model, bool generateRefreshToken = true)
    {
        var user = await userManager.FindByNameAsync(model.Username);
        return user is not null && await userManager.CheckPasswordAsync(user, model.Password)
            ? await GenerateToken(user, generateRefreshToken)
            : throw new IdentityException(ExceptionMessages.IncorrectUserPass);
    }

    public async Task<TokenResponse> GenerateToken(string token, string refreshToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        if (!ValidToken(token))
            throw new IdentityException(ExceptionMessages.InvalidToken);

        var jwtSecurityToken = tokenHandler.ReadJwtToken(token);
        var username = jwtSecurityToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        if (string.IsNullOrEmpty(username))
            throw new IdentityException(ExceptionMessages.InvalidToken);

        var user = await userManager.FindByNameAsync(username);
        if (user is null)
            throw new IdentityException(ExceptionMessages.UserNotFound);
        else if (!(user.RefreshToken?.Equals(refreshToken) ?? false))
            throw new IdentityException(ExceptionMessages.InvalidRefreshToken);
        else if (user.RefreshTokenExpiryDate < DateTime.Now)
            throw new IdentityException(ExceptionMessages.RefreshTokenExpired);

        return await GenerateToken(user, false);
    }

    public async Task<ResponseMessage> RemoveRefreshToken(string username)
    {
        if (string.IsNullOrEmpty(username))
            throw new IdentityException(ExceptionMessages.UserNotFound);

        var user = await userManager.FindByNameAsync(username) ?? throw new IdentityException(ExceptionMessages.UserNotFound);
        user.RefreshTokenExpiryDate = null;
        user.RefreshToken = null;
        var result = await userManager.UpdateAsync(user);

        return !result.Succeeded
            ? throw new IdentityException(ExceptionMessages.RefreshTokenRemoveFailed)
            : new ResponseMessage("000", "Removed refresh token");
    }

    private async Task<TokenResponse> GenerateToken(TUser user, bool generateRefreshToken = true)
    {
        var now = DateTime.Now;
        var userRoles = await userManager.GetRolesAsync(user);

        var claims = new List<Claim>
                    {
                        new(ClaimTypes.Name, user.UserName),
                        new(ClaimTypes.NameIdentifier, user.Id),
                        new(ClaimTypes.Email, user.Email),
                        new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
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
        if (generateRefreshToken)
        {
            user.RefreshToken = GenerateRefreshToken();
            user.RefreshTokenExpiryDate = now.AddHours(options.Value.RefreshTokenExpiryTime);
            await userManager.UpdateAsync(user);
        }

        return new TokenResponse
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = token.ValidTo,
            RefreshToken = user.RefreshToken,
            RefreshTokenExpiration = user.RefreshTokenExpiryDate,
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

    internal abstract SecurityKey GetSecurityKey();

    private JwtSecurityToken GetJwtSecurityToken(DateTime now, List<Claim> claims)
    {
        return new JwtSecurityToken(
            issuer: options.Value.ValidIssuer,
            audience: options.Value.ValidAudience,
            expires: now.AddHours(options.Value.ExpiryTime),
            claims: claims,
            signingCredentials: new SigningCredentials(GetSecurityKey(), options.Value.SecurityAlgorithm)
            );
    }

    private bool ValidToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        if (!tokenHandler.CanReadToken(token))
            return false;

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = options.Value.ValidIssuer,
            ValidateAudience = true,
            ValidAudience = options.Value.ValidAudience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = GetSecurityKey(),
            ValidateLifetime = false,
            ClockSkew = TimeSpan.Zero
        };

        try
        {
            ClaimsPrincipal principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
            return true;
        }
        catch (System.Exception)
        {
            return false;
        }
    }

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}
