using Autofac;
using MediatR;
using NetCore.Abstraction;
using NetCore.Abstraction.Model;
using System.Diagnostics;

namespace NetCore.Container
{
    public class LoggingHandler<TRequest, TResponse> : DecoratorBase<TRequest, TResponse> where TRequest : IRequest<TResponse> where TResponse : ResponseBase
    {
        private readonly ILifetimeScope _lifetimeScope;
        private readonly IAppLogger _appLogger;

        public LoggingHandler(ILifetimeScope lifetimeScope, IAppLogger appLogger)
        {
            _lifetimeScope = lifetimeScope;
            _appLogger = appLogger;
        }

        public override async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var methodInfo = GetHandlerMethodInfo(_lifetimeScope);

            _appLogger.EntryLog(request, methodInfo);

            var timer = new Stopwatch();
            timer.Start();

            var response = await next();

            timer.Stop();
            _appLogger.ExitLog(response, methodInfo, timer.ElapsedMilliseconds);

            return response;
        }
    }
}
