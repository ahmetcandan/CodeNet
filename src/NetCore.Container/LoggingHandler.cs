using Autofac;
using MediatR;
using NetCore.Abstraction;
using NetCore.Abstraction.Model;
using System.Diagnostics;

namespace NetCore.Container;

public class LoggingHandler<TRequest, TResponse>(ILifetimeScope lifetimeScope, IAppLogger appLogger) : DecoratorBase<TRequest, TResponse> where TRequest : IRequest<TResponse> where TResponse : ResponseBase
{
    public override async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var methodInfo = GetHandlerMethodInfo(lifetimeScope);

        appLogger.EntryLog(request, methodInfo);

        var timer = new Stopwatch();
        timer.Start();

        var response = await next();

        timer.Stop();
        appLogger.ExitLog(response, methodInfo, timer.ElapsedMilliseconds);

        return response;
    }
}
