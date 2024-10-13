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
            return await dbContext.Database.CanConnectAsync(cancellationToken)
                ? HealthCheckResult.Healthy($"This is {providerName}, standing as always. Have a good work ;) ")
                : HealthCheckResult.Unhealthy($"Sorry, {providerName} is down :(");
        }
        catch
        {
            return HealthCheckResult.Unhealthy("Sorry, SqlServer is down :(");
        }
    }
}

internal class EntityFrameworkHealthCheck(DbContextOptions options) : EntityFrameworkHealthCheck<DbContext>(new DbContext(options))
{
}
