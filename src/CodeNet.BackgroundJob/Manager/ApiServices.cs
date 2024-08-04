using CodeNet.BackgroundJob.Models;
using CodeNet.EntityFramework.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace CodeNet.BackgroundJob.Manager;

internal static class ApiServices
{
    public static void GetServices(this WebApplication app, IServiceProvider serviceProvider, string path)
    {
        app.MapGet($"{path}/getServices", async (int page, int count, CancellationToken cancellationToken) =>
        {
            var dbContext = serviceProvider.GetRequiredService<BackgroundJobDbContext>();
            var serviceRepository = new Repository<Job>(dbContext);
            return await serviceRepository.GetPagingListAsync(page, count, cancellationToken);
        });
    }

    public static void GetServiceDetails(this WebApplication app, IServiceProvider serviceProvider, string path)
    {
        app.MapGet($"{path}/getServiceDetails", async (int jobId, DetailStatus? status, int page, int count, CancellationToken cancellationToken) =>
        {
            var dbContext = serviceProvider.GetRequiredService<BackgroundJobDbContext>();
            var jobWorkingDetailRepository = new Repository<JobWorkingDetail>(dbContext);
            return await jobWorkingDetailRepository.GetPagingListAsync(c => c.JobId == jobId && (status == null || c.Status == status), c => c.StartDate, false, page, count);
        });
    }

    public static void DeleteDetails(this WebApplication app, IServiceProvider serviceProvider, string path)
    {
        app.MapDelete($"{path}/deleteDetails", async ([FromBody] int[] detailIds, CancellationToken cancellationToken) =>
        {
            var dbContext = serviceProvider.GetRequiredService<BackgroundJobDbContext>();
            var jobWorkingDetailRepository = new Repository<JobWorkingDetail>(dbContext);
            var list = await jobWorkingDetailRepository.FindAsync(c => detailIds.Any(i => c.Id == i), cancellationToken);
            jobWorkingDetailRepository.RemoveRange(list);
            return await dbContext.SaveChangesAsync(cancellationToken);
        });
    }

    public static void ChangeServiceStatus(this WebApplication app, IServiceProvider serviceProvider, string path)
    {
        app.MapPut($"{path}/changeStatus", async (int jobId, [FromBody] JobStatus status, CancellationToken cancellationToken) =>
        {
            var dbContext = serviceProvider.GetRequiredService<BackgroundJobDbContext>();
            var serviceRepository = new Repository<Job>(dbContext);
            var jobEntity = await serviceRepository.GetAsync([jobId], cancellationToken) ?? throw new Exception("Job entity is not found!");
            var tJob = serviceProvider.GetServices<IScheduleJob>().FirstOrDefault(c => c.GetType().ToString() == jobEntity.ServiceName) ?? throw new Exception($"{jobEntity.ServiceName} IScheduleJob is not found!");

            var tJobType = tJob.GetType();
            var serviceType = typeof(ICodeNetBackgroundService<>).MakeGenericType(tJobType);
            var backgroundService = serviceProvider.GetService(serviceType) as ICodeNetBackgroundService ?? throw new NotImplementedException($"'builder.services.AddBackgroundService<{tJobType.Name}>(string cronExpression, TimeSpan? lockExperyTime = null)' not implemented background service.");

            var currentStatus = backgroundService.GetStatus();
            switch (status)
            {
                case JobStatus.Stopped:
                    if (currentStatus == JobStatus.Stopped)
                        return $"The {jobEntity.ServiceName} service is already stopped.";

                    await backgroundService.StopAsync(cancellationToken);
                    return $"{jobEntity.ServiceName} service has been stopped.";
                case JobStatus.Running:
                    if (currentStatus == JobStatus.Stopped)
                    {
                        app.Lifetime.ApplicationStarted.Register(async () => await backgroundService.StartAsync(CancellationToken.None));
                        return $"{jobEntity.ServiceName} service has been started.";
                    }

                    return $"The {jobEntity.ServiceName} service is already running.";

                default:
                    throw new NotImplementedException();
            }
        });
    }

    public static void GetJobStatus(this WebApplication app, IServiceProvider serviceProvider, string path)
    {
        app.MapGet($"{path}/getJobStatus", async (int jobId, CancellationToken cancellationToken) =>
        {
            var dbContext = serviceProvider.GetRequiredService<BackgroundJobDbContext>();
            var serviceRepository = new Repository<Job>(dbContext);
            var jobEntity = await serviceRepository.GetAsync([jobId], cancellationToken) ?? throw new Exception("Job entity is not found!");
            var tJob = serviceProvider.GetServices<IScheduleJob>().FirstOrDefault(c => c.GetType().ToString() == jobEntity.ServiceName) ?? throw new Exception($"{jobEntity.ServiceName} IScheduleJob is not found!");

            var tJobType = tJob.GetType();
            var serviceType = typeof(ICodeNetBackgroundService<>).MakeGenericType(tJobType);
            var backgroundService = serviceProvider.GetService(serviceType) as ICodeNetBackgroundService ?? throw new NotImplementedException($"'builder.services.AddBackgroundService<{tJobType.Name}>(string cronExpression, TimeSpan? lockExperyTime = null)' not implemented background service.");

            var currentStatus = backgroundService.GetStatus();
            return new
            {
                Status = (int)currentStatus,
                StatusName = currentStatus.ToString()
            };
        });
    }

    public static void JobExecute(this WebApplication app, IServiceProvider serviceProvider, string path)
    {
        app.MapPost($"{path}/jobExecute", async (int jobId, CancellationToken cancellationToken) =>
        {
            var dbContext = serviceProvider.GetRequiredService<BackgroundJobDbContext>();
            var serviceRepository = new Repository<Job>(dbContext);
            var jobEntity = await serviceRepository.GetAsync([jobId], cancellationToken) ?? throw new Exception("Job entity is not found!");
            var tJob = serviceProvider.GetServices<IScheduleJob>().FirstOrDefault(c => c.GetType().ToString() == jobEntity.ServiceName) ?? throw new Exception($"{jobEntity.ServiceName} IScheduleJob is not found!");

            var tJobType = tJob.GetType();
            var serviceType = typeof(ICodeNetBackgroundService<>).MakeGenericType(tJobType);
            var backgroundService = serviceProvider.GetService(serviceType) as ICodeNetBackgroundService ?? throw new NotImplementedException($"'builder.services.AddBackgroundService<{tJobType.Name}>(string cronExpression, TimeSpan? lockExperyTime = null)' not implemented background service.");

            return await backgroundService.DoWorkAsync(tJob, jobId, cancellationToken);
        });
    }
}
