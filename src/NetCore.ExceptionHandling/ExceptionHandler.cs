using MediatR;
using NetCore.Abstraction;
using NetCore.Abstraction.Model;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NetCore.ExceptionHandling
{
    public class ExceptionHandler<TRequest, TResponse> : DecoratorBase<TRequest, TResponse> where TResponse : ResponseBase, new() where TRequest : IRequest<TResponse>
    {
        public ExceptionHandler()
        {
        }

        public override async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            TResponse response = null;
            var handlerMethodInfo = GetHandlerMethodInfo();
            try
            {
                response = await next();
            }
            catch (Exception exception)
            {
                //_appLogger.Exception(exception, handlerMethodInfo);

                //switch (exception)
                //{
                //    case BusinessRuleException businessRuleException:
                //        response = new TResponse
                //        {
                //            UserMessage = businessRuleException.UserMessage,
                //            Message = businessRuleException.Message,
                //            MessageCode = businessRuleException.Code
                //        };
                //        break;
                //    //http timeout
                //    case TaskCanceledException:
                //    case AggregateException:
                //        response = new TResponse
                //        {
                //            UserMessage = ApplicationMessage.TimeoutOccurred.UserMessage(),
                //            Message = ApplicationMessage.TimeoutOccurred.Message(),
                //            MessageCode = ApplicationMessage.TimeoutOccurred
                //        };
                //        break;
                //    //unexpected http response received
                //    case HttpRequestException:
                //    case JsonReaderException:
                //        response = new TResponse
                //        {
                //            UserMessage = ApplicationMessage.UnExpectedHttpResponseReceived.UserMessage(),
                //            Message = ApplicationMessage.UnExpectedHttpResponseReceived.Message(),
                //            MessageCode = ApplicationMessage.UnExpectedHttpResponseReceived
                //        };
                //        break;
                //    default:
                //        response = new TResponse
                //        {
                //            UserMessage = ApplicationMessage.UnhandledError.UserMessage(),
                //            Message = ApplicationMessage.UnhandledError.Message(),
                //            MessageCode = ApplicationMessage.UnhandledError
                //        };
                //        break;
                //}
            }

            return response;
        }
    }
}
