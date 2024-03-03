using MediatR;
using NetCore.Abstraction.Model;
using System.Diagnostics;

namespace StokTakip.Customer.Container.Decorator
{
    public class LoggingHandler<TRequest, TResponse> : DecoratorBase<TRequest, TResponse> where TRequest : IRequest<TResponse> where TResponse : ResponseBase
    {
        public LoggingHandler()
        {
            
        }

        public override async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var metodInfo = GetHandlerMethodInfo();

            Console.WriteLine($"{metodInfo.Name} giriş");

            var timer = new Stopwatch();
            timer.Start();

            var response = await next();

            Console.WriteLine($"{metodInfo.Name} çıkış");

            timer.Stop();

            return response;
        }
    }
}
