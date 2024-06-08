using Autofac;
using MediatR;
using NetCore.Abstraction;
using NetCore.Abstraction.Model;
using NetCore.Container;
using System.Diagnostics;

namespace NetCore.Logging;

public class LoggingHandler<TRequest, TResponse>(ILifetimeScope LifetimeScope, IAppLogger AppLogger) : DecoratorBase<TRequest, TResponse> where TRequest : IRequest<TResponse> where TResponse : ResponseBase
{
    public override async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var methodInfo = GetHandlerMethodInfo(LifetimeScope);

        AppLogger.EntryLog(request, methodInfo!);

        var timer = new Stopwatch();
        timer.Start();

        var response = await next();

        timer.Stop();
        AppLogger.ExitLog(response, methodInfo!, timer.ElapsedMilliseconds);

        return response;
    }
}
