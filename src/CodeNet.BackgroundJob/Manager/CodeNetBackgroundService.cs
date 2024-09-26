using CodeNet.BackgroundJob.Models;
using CodeNet.BackgroundJob.Settings;
using CodeNet.EntityFramework.Repositories;
using CodeNet.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Reflection;

namespace CodeNet.BackgroundJob.Manager;

internal class CodeNetBackgroundService<TJob>(IOptions<JobOptions<TJob>> options, IServiceProvider serviceProvider) : ICodeNetBackgroundService<TJob>
    where TJob : class, IScheduleJob
{
    private bool _exit = false;
    private readonly bool _manuelExecute = options.Value.Cron is null && options.Value.PeriodTime is null;
    private bool _manuelRunning = false;
    private Stopwatch? _stopwatch = null;
    private TimeSpan? _manuelTime;
    private JobStatus _jobStatus = JobStatus.Pending;
    private int _jobId;
    private const int _manuelDelaySeconds = 60;

    public void SetManuelTime(TimeSpan manuelTimeSpan)
    {
        if (!_manuelExecute)
            return;

        _manuelTime = manuelTimeSpan;
        _stopwatch = Stopwatch.StartNew();
    }

    public void SetManuelTime(DateTime manuelDateTime)
    {
        if (!_manuelExecute)
            return;

        var now = DateTime.Now;
        if (manuelDateTime > now)
        {
            _stopwatch = Stopwatch.StartNew();
            _manuelTime = manuelDateTime - now;
        }
    }

    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        _exit = false;
        await AddOrUpdateService(cancellationToken);

        var tJob = serviceProvider.GetServices<IScheduleJob>().FirstOrDefault(c => c.GetType().Equals(typeof(TJob)));
        if (tJob is null)
        {
            _jobStatus = JobStatus.Stopped;
            MessageInvoke();
            return;
        }

        while (!cancellationToken.IsCancellationRequested && !_exit)
        {
            using var serviceScope = serviceProvider.CreateAsyncScope();
            var now = DateTime.UtcNow;
            TimeSpan timeSpan;
            if (options.Value.Cron is not null)
            {
                var nextTime = options.Value.Cron.GetNextOccurrence(now, TimeZoneInfo.Local);
                timeSpan = (nextTime - now) ?? new TimeSpan(0);
            }
            else if (options.Value.PeriodTime is not null)
            {
                timeSpan = options.Value.PeriodTime.Value;
            }
            else if (_manuelTime.HasValue)
            {
                _stopwatch?.Stop();
                timeSpan = _manuelTime.Value.Subtract(new TimeSpan(_stopwatch?.ElapsedTicks ?? 0));
                _manuelRunning = true;
            }
            else
            {
                timeSpan = new TimeSpan((_manuelDelaySeconds * 10000000) - ((now.Second * 10000000) + (now.Millisecond * 10000) + (now.Microsecond * 10) + (now.Nanosecond / 100)));
                _manuelRunning = false;
            }
            await Task.Delay(timeSpan, cancellationToken);
            var result = await DoWorkAsync(tJob, _jobId, cancellationToken);

            if (_manuelRunning)
                _manuelTime = null;
            _manuelRunning = false;

            if (result is { Status: DetailStatus.Fail or DetailStatus.Stopped })
            {
                _jobStatus = JobStatus.Stopped;
                MessageInvoke();
                break;
            }

            _jobStatus = JobStatus.Pending;
            MessageInvoke();
        }
    }

    private async Task AddOrUpdateService(CancellationToken cancellationToken)
    {
        using var serviceScope = serviceProvider.CreateScope();
        var dbContext = serviceScope.ServiceProvider.GetService<BackgroundJobDbContext>();

        if (dbContext is null)
            return;

        var serviceRepository = new Repository<Job>(dbContext);
        var currentJob = await serviceRepository.GetAsync(c => c.ServiceType == options.Value.ServiceType, cancellationToken);
        if (currentJob is null)
        {
            var jobEntity = await serviceRepository.AddAsync(new Job
            {
                PeriodTime = options.Value.PeriodTime,
                CronExpression = options.Value.Cron?.ToString(),
                ExpryTime = options.Value.ExpryTime,
                ServiceType = options.Value.ServiceType,
                Title = options.Value.ServiceType,
                Status = JobStatus.Pending,
                IsActive = true
            }, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            _jobId = jobEntity.Id;
        }
        else
        {
            currentJob.Status = JobStatus.Pending;
            currentJob.ExpryTime = options.Value.ExpryTime;
            currentJob.PeriodTime = options.Value.PeriodTime;
            currentJob.CronExpression = options.Value.Cron?.ToString();
            currentJob.ServiceType = options.Value.ServiceType;
            currentJob.Title = options.Value.Title;
            currentJob.IsActive = true;
            serviceRepository.Update(currentJob);
            await dbContext.SaveChangesAsync(cancellationToken);
            _jobId = currentJob.Id;
        }
    }

    public async Task<JobWorkingDetail?> DoWorkAsync(IScheduleJob tJob, int jobId, CancellationToken cancellationToken = default)
    {
        if (_manuelExecute && !_manuelRunning)
            return null;

        DateTimeOffset? startDate = null;
        var methodInfo = MethodBase.GetCurrentMethod();
        using var serviceScope = serviceProvider.CreateScope();
        var appLogger = serviceScope.ServiceProvider.GetService<IAppLogger>();
        var dbContext = serviceScope.ServiceProvider.GetService<BackgroundJobDbContext>();
        var workingDetailRepository = dbContext is not null ? new Repository<JobWorkingDetail>(dbContext) : null;
        try
        {
            var distributedLock = serviceScope.ServiceProvider.GetService<IJobLock>();
            if (distributedLock is not null)
            {
                using var redLock = await distributedLock.CreateLock(typeof(TJob).ToString(), options.Value.ExpryTime ?? TimeSpan.FromSeconds(10));
                if (!redLock.IsAcquired)
                    return null;
            }

            if (_exit)
            {
                _jobStatus = JobStatus.Stopped;
                var detail = new JobWorkingDetail
                {
                    Message = "Not working. This Job has been stopped.",
                    Status = DetailStatus.Stopped,
                    JobId = jobId
                };

                if (workingDetailRepository is not null)
                    await workingDetailRepository.AddAsync(detail, cancellationToken);

                MessageInvoke(detail);

                if (workingDetailRepository is not null)
                    await workingDetailRepository.SaveChangesAsync(cancellationToken);

                return detail;
            }
            else
            {
                appLogger?.EntryLog($"'{typeof(TJob)}' starting scheduled task.", methodInfo);
                var timer = new Stopwatch();
                timer.Start();
                startDate = DateTimeOffset.Now;
                var currentStatus = _jobStatus;
                _jobStatus = JobStatus.Running;
                MessageInvoke();
                await tJob.Execute(cancellationToken);
                _jobStatus = currentStatus;
                timer.Stop();
                appLogger?.ExitLog($"'{typeof(TJob)}' scheduled mission is over.", methodInfo, timer.ElapsedMilliseconds);
                var detail = new JobWorkingDetail
                {
                    Message = $"Job worked successfully.",
                    Status = DetailStatus.Successful,
                    StartDate = startDate,
                    ElapsedTime = TimeSpan.FromMicroseconds(timer.ElapsedMilliseconds),
                    JobId = jobId
                };

                if (workingDetailRepository is not null)
                    await workingDetailRepository.AddAsync(detail, cancellationToken);

                MessageInvoke(detail);

                if (workingDetailRepository is not null)
                    await workingDetailRepository.SaveChangesAsync(cancellationToken);

                return detail;
            }
        }
        catch (Exception ex)
        {
            appLogger?.ExceptionLog(ex, methodInfo);
            _jobStatus = JobStatus.Stopped;
            var detail = new JobWorkingDetail
            {
                Message = $"Thrown exception while Job was working. Exeption: {ex.Message}",
                Status = DetailStatus.Fail,
                StartDate = startDate,
                JobId = jobId
            };

            if (workingDetailRepository is not null)
                await workingDetailRepository.AddAsync(detail, cancellationToken);

            MessageInvoke(detail);

            if (workingDetailRepository is not null)
                await workingDetailRepository.SaveChangesAsync(cancellationToken);

            return detail;
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken = default)
    {
        using var serviceScope = serviceProvider.CreateScope();
        var dbContext = serviceScope.ServiceProvider.GetService<BackgroundJobDbContext>();
        if (dbContext is not null)
        {
            var serviceRepository = new Repository<Job>(dbContext);
            var service = await serviceRepository.GetAsync(c => c.ServiceType == typeof(TJob).ToString(), cancellationToken);
            if (service is not null)
            {
                service.Status = JobStatus.Stopped;
                serviceRepository.Update(service);
                await serviceRepository.SaveChangesAsync(cancellationToken);
            }
        }
        var appLogger = serviceScope.ServiceProvider.GetService<IAppLogger>();
        appLogger?.TraceLog($"'{nameof(TJob)}' stoped scheduled task.", MethodBase.GetCurrentMethod());
        _exit = true;
        _jobStatus = JobStatus.Stopped;
        MessageInvoke();
    }

    public JobStatus GetStatus()
    {
        return _jobStatus;
    }

    public int GetJobId()
    {
        return _jobId;
    }

    public event StatusChanged? StatusChanged;

    private void MessageInvoke(JobWorkingDetail? detail = null)
    {
        StatusChanged?.Invoke(new ReceivedMessageEventArgs
        {
            Detail = detail,
            ServiceName = options.Value.Title,
            Status = _jobStatus,
            JobId = _jobId
        });
    }
}
