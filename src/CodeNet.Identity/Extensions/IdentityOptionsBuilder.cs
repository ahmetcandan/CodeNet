using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace CodeNet.Identity.Extensions;

public class IdentityOptionsBuilder(IServiceCollection services)
{
    public IdentityOptionsBuilder AddPasswordHasher<TPasswordHasher, TUser>()
        where TPasswordHasher : class, IPasswordHasher<TUser>
        where TUser : class
    {
        services.AddScoped<IPasswordHasher<TUser>, TPasswordHasher>();
        return this;
    }
}
