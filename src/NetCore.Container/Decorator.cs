using Autofac;
using MediatR;
using NetCore.Abstraction.Model;
using System.Reflection;

namespace NetCore.Container;

public abstract class DecoratorBase<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
     where TResponse : ResponseBase where TRequest : IRequest<TResponse>
{
    public abstract Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken);


    protected MethodBase? GetHandlerMethodInfo(ILifetimeScope lifetimeScope)
    {
        var handler = lifetimeScope.Resolve<IRequestHandler<TRequest, TResponse>>();
        return handler != null ? handler.GetType().GetMethod("Handle") : (MethodBase?)null;
    }
}
