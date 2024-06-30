using Autofac;
using CodeNet.Core;
using CodeNet.Core.Models;
using MediatR;
using System.Reflection;

namespace CodeNet.Parameters.Handler;

internal class ParameterDecorator<TRequest, TResponse> : DecoratorBase<TRequest, TResponse>
    where TResponse : ResponseBase
    where TRequest : IRequest<TResponse>
{
    public override Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public static new MethodBase? GetHandlerMethodInfo(ILifetimeScope lifetimeScope)
    {
        return DecoratorBase<TRequest, TResponse>.GetHandlerMethodInfo(lifetimeScope);
    }

    public static new string GetKey(MethodBase methodBase, TRequest request)
    {
        return DecoratorBase<TRequest, TResponse>.GetKey(methodBase, request);
    }
}
