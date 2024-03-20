using Autofac;
using MediatR;
using NetCore.Abstraction;
using NetCore.Abstraction.Model;
using NetCore.ExceptionHandling;

namespace NetCore.Container;

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
            AppLogger.ExceptionLog(ex, GetHandlerMethodInfo(LifetimeScope));
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
