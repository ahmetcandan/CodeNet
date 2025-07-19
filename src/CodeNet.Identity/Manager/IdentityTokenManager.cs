using CodeNet.Core.Models;
using CodeNet.Identity.DbContext;
using CodeNet.Identity.Exception;
using CodeNet.Identity.Models;
using CodeNet.Identity.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace CodeNet.Identity.Manager;

internal abstract class IdentityTokenManager<TUser, TRole, TKey>(UserManager<TUser> userManager, RoleManager<TRole> roleManager, CodeNetIdentityDbContext<TUser, TRole, TKey> dbContext, IOptions<BaseIdentitySettings> options) : IIdentityTokenManager
    where TUser : IdentityUser<TKey>
    where TRole : IdentityRole<TKey>
    where TKey : IEquatable<TKey>
{
    protected readonly DbSet<UserRefreshToken<TKey>> _userRefreshTokens = dbContext.Set<UserRefreshToken<TKey>>();

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

        var user = await userManager.FindByNameAsync(username) ?? throw new IdentityException(ExceptionMessages.UserNotFound);
        var refreshTokenModel = await GetActiveRefreshToken(user.Id, refreshToken);
        return refreshTokenModel is null || !(refreshTokenModel?.RefreshToken?.Equals(refreshToken) ?? false)
            ? throw new IdentityException(ExceptionMessages.InvalidRefreshToken)
            : await GenerateToken(user, false);
    }

    public async Task<ResponseMessage> RemoveRefreshToken(string username)
    {
        if (string.IsNullOrEmpty(username))
            throw new IdentityException(ExceptionMessages.UserNotFound);

        var user = await userManager.FindByNameAsync(username) ?? throw new IdentityException(ExceptionMessages.UserNotFound);
        await RemoveRefreshTokenById(user.Id);
        return new ResponseMessage("000", "Removed refresh token");
    }

    private async Task<TokenResponse> GenerateToken(TUser user, bool generateRefreshToken = true)
    {
        if (user is null) throw new IdentityException(ExceptionMessages.UserNotFound);

        var now = DateTime.Now;
        var userRoles = await userManager.GetRolesAsync(user);

        var claims = new List<Claim>
                    {
                        new(ClaimTypes.Name, user.UserName!),
                        new(ClaimTypes.NameIdentifier, user.Id.ToString()!),
                        new(ClaimTypes.Email, user.Email!),
                        new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                    };

        claims.AddRange(await userManager.GetClaimsAsync(user));
        foreach (var roleName in userRoles)
        {
            claims.Add(new Claim(ClaimTypes.Role, roleName));
            var role = await roleManager.FindByNameAsync(roleName);
            if (role is not null)
                claims.AddRange(await roleManager.GetClaimsAsync(role));
        }

        claims.Add(new Claim("LoginTime", now.ToString("O"), "DateTime[O]"));

        var token = GetJwtSecurityToken(now, claims);
        UserRefreshToken<TKey>? userRefreshToken = null;
        if (generateRefreshToken)
        {
            userRefreshToken = await GetUserRefreshToken(user.Id);
            if (userRefreshToken is null)
            {
                userRefreshToken = new UserRefreshToken<TKey>
                {
                    UserId = user.Id,
                    RefreshToken = GenerateRefreshToken(),
                    RefreshTokenExpiryDate = now.AddHours(options.Value.RefreshTokenExpiryTime)
                };
                await _userRefreshTokens.AddAsync(userRefreshToken);
            }
            else
            {
                userRefreshToken.RefreshToken = GenerateRefreshToken();
                userRefreshToken.RefreshTokenExpiryDate = now.AddHours(options.Value.RefreshTokenExpiryTime);
                _userRefreshTokens.Update(userRefreshToken);
            }
            await dbContext.SaveChangesAsync();
        }

        return new TokenResponse
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = token.ValidTo,
            RefreshToken = userRefreshToken?.RefreshToken,
            RefreshTokenExpiration = userRefreshToken?.RefreshTokenExpiryDate,
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
            _ = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
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

    private async Task<RefreshTokenModel<TKey>?> GetActiveRefreshToken(TKey userId, string refreshToken)
    {
        var result = await GetUserRefreshToken(userId, refreshToken);
        return result is null
            ? null
            : new()
            {
                UserId = userId,
                RefreshToken = result?.RefreshToken ?? string.Empty,
                ExpiryTime = result?.RefreshTokenExpiryDate ?? DateTime.MinValue
            };
    }

    private Task<UserRefreshToken<TKey>?> GetUserRefreshToken(TKey userId, string refreshToken) => _userRefreshTokens.FirstOrDefaultAsync(c => c.UserId.Equals(userId) && c.RefreshToken.Equals(refreshToken) && c.RefreshTokenExpiryDate > DateTime.Now);

    private Task<UserRefreshToken<TKey>?> GetUserRefreshToken(TKey userId) => _userRefreshTokens.FirstOrDefaultAsync(c => c.UserId.Equals(userId) && c.RefreshTokenExpiryDate > DateTime.Now);

    private async Task RemoveRefreshTokenById(TKey userId)
    {
        var refreshTokens = await _userRefreshTokens.Where(c => c.UserId.Equals(userId)).ToListAsync();
        if (refreshTokens is not null)
        {
            _userRefreshTokens.RemoveRange(refreshTokens);
            await dbContext.SaveChangesAsync();
        }
    }
}
