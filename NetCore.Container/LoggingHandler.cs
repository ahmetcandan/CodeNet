using Autofac;
using MediatR;
using NetCore.Abstraction.Model;
using System.Diagnostics;

namespace NetCore.Container
{
    public class LoggingHandler<TRequest, TResponse> : DecoratorBase<TRequest, TResponse> where TRequest : IRequest<TResponse> where TResponse : ResponseBase
    {
        private readonly ILifetimeScope _lifetimeScope;

        public LoggingHandler(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
        }

        public override async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var metodInfo = GetHandlerMethodInfo(_lifetimeScope);

            Console.WriteLine($"{metodInfo?.Name} giriş");

            var timer = new Stopwatch();
            timer.Start();

            var response = await next();

            Console.WriteLine($"{metodInfo?.Name} çıkış");

            timer.Stop();

            return response;
        }
    }
}
