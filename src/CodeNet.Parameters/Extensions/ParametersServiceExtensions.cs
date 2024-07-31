using CodeNet.Core.Extensions;
using CodeNet.MakerChecker.Extensions;
using CodeNet.Parameters.Manager;
using CodeNet.Parameters.Settings;
using CodeNet.Redis.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CodeNet.Parameters.Extensions;

public static class ParametersServiceExtensions
{
    /// <summary>
    /// Add Parameters by EF Core
    /// </summary>
    /// <param name="services"></param>
    /// <param name="optionsAction"></param>
    /// <param name="redisSectionName"></param>
    /// <returns></returns>
    public static IServiceCollection AddParameters(this IServiceCollection services, Action<ParameterOptionsBuilder> action, IConfigurationSection? parameterSection = null)
    {
        if (parameterSection is not null)
            services.Configure<ParameterSettings>(parameterSection);
        else
            services.Configure<ParameterSettings>(c => { });

        if (action is not null)
        {
            var builder = new ParameterOptionsBuilder(services);
            action(builder);
        }

        if (!services.Any(c => c.ServiceType.Equals(typeof(IParameterManager))))
            services.AddScoped<IParameterManager, ParameterManager>();

        return services.AddCodeNetContext();
    }
}
