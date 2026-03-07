using CodeNet.Core.Models;

namespace CodeNet.BackgroundJob.Exception;

internal static class ExceptionMessages
{
    public static ExceptionMessage JobEntityNotFound { get { return new ExceptionMessage("BJ0001", "Job Entity not found!"); } }
    public static ExceptionMessage IScheduleJobNotFound { get { return new ExceptionMessage("BJ0002", "IScheduleJob not found!"); } }

    public static ExceptionMessage UseParams(this ExceptionMessage exceptionMessage, params string[] args) => new(exceptionMessage.Code, string.Format(exceptionMessage.Message, args));
}
