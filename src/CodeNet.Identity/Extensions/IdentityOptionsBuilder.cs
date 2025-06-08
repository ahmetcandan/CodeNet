using CodeNet.Identity.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace CodeNet.Identity.Extensions;

public class IdentityOptionsBuilder(IServiceCollection services)
{
    public IdentityOptionsBuilder AddPasswordHasher<TPasswordHasher>()
        where TPasswordHasher : class, IPasswordHasher<ApplicationUser>
    {
        services.AddScoped<IPasswordHasher<ApplicationUser>, TPasswordHasher>();
        return this;
    }
}
