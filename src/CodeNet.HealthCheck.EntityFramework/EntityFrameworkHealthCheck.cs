using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CodeNet.HealthCheck.EntityFramework;

internal class EntityFrameworkHealthCheck<TDbContext>(TDbContext dbContext) : IHealthCheck
    where TDbContext : DbContext
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var providerName = dbContext.Database.ProviderName?.Split('.')[^1];
            if (await dbContext.Database.CanConnectAsync(cancellationToken))
                return HealthCheckResult.Healthy($"This is {providerName}, standing as always. Have a good work ;) ");

            return HealthCheckResult.Unhealthy($"Sorry, {providerName} is down :(");
        }
        catch
        {
            return HealthCheckResult.Unhealthy("Sorry, SqlServer is down :(");
        }
    }
}
