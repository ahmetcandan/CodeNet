using Autofac;
using MediatR;
using CodeNet.Abstraction;
using CodeNet.Abstraction.Model;
using CodeNet.Container;

namespace CodeNet.ExceptionHandling;

public class ExceptionHandler<TRequest, TResponse>(ILifetimeScope LifetimeScope, IAppLogger AppLogger) : DecoratorBase<TRequest, TResponse> where TRequest : IRequest<TResponse> where TResponse : ResponseBase, new()
{
    public override async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            AppLogger.ExceptionLog(ex, GetHandlerMethodInfo(LifetimeScope)!);
            return ex switch
            {
                UserLevelException userLevelException => new TResponse
                {
                    IsSuccessfull = false,
                    Message = userLevelException.Message,
                    MessageCode = userLevelException.Code
                },
                _ => new TResponse
                {
                    IsSuccessfull = false,
                    Message = "Unexpected error",
                    MessageCode = "00"
                },
            };
        }
    }
}
