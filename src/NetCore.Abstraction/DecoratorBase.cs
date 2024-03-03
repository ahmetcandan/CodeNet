using MediatR;
using NetCore.Abstraction.Model;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace NetCore.Abstraction
{
    public abstract class DecoratorBase<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
         where TResponse : ResponseBase where TRequest : IRequest<TResponse>
    {
        public abstract Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken);


        protected MethodBase GetHandlerMethodInfo()
        {
            //var handler = Bootstrapper.Container.Resolve<IRequestHandler<TRequest, TResponse>>();
            //if (handler != null)
            //{
            //    return handler.GetType().GetMethod("Handle");
            //}

            return null;
        }
    }
}
