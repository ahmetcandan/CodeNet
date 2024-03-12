using Autofac;
using MediatR;
using NetCore.Abstraction;
using NetCore.Abstraction.Model;
using NetCore.ExceptionHandling;

namespace NetCore.Container;

public class ExceptionHandler<TRequest, TResponse>(ILifetimeScope lifetimeScope, IAppLogger appLogger) : DecoratorBase<TRequest, TResponse> where TRequest : IRequest<TResponse> where TResponse : ResponseBase, new()
{
    public override async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var methodInfo = GetHandlerMethodInfo(lifetimeScope);

        try
        {
            var response = await next();
            return response;
        }
        catch (Exception ex)
        {
            appLogger.ExceptionLog(ex, methodInfo);
            switch (ex)
            {
                case UserLevelException userLevelException:
                    return new TResponse
                    {
                        IsSuccessfull = false,
                        Message = userLevelException.Message,
                        MessageCode = userLevelException.Code
                    };
                default:
                    return new TResponse
                    {
                        IsSuccessfull = false,
                        Message = "Unexpected error",
                        MessageCode = "00"
                    };
            }
        }
    }
}
