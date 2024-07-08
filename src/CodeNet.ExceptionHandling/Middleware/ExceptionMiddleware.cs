using CodeNet.Core.Models;
using CodeNet.ExceptionHandling.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace CodeNet.ExceptionHandling;

internal class ExceptionHandlerMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            context.Response.ContentType = "application/json";
            string? errorCode = null, errorMessage = null;
            switch (ex)
            {
                case UserLevelException userLevelException:
                    errorCode = userLevelException.Code;
                    errorMessage = userLevelException.Message;
                    context.Response.StatusCode = userLevelException?.HttpStatusCode ?? (int)HttpStatusCode.InternalServerError;
                    break;
                case CodeNetException codeNetException:
                    context.Response.StatusCode = codeNetException?.HttpStatusCode ?? (int)HttpStatusCode.InternalServerError;
                    break;
                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            var configuration = context.RequestServices.GetRequiredService<IConfiguration>();
            var defaultErrorMessage = configuration.Get<ErrorResponseMessage>();
            await context.Response.WriteAsJsonAsync(defaultErrorMessage ?? new ResponseMessage(errorCode ?? "EX0001", errorMessage ?? "An unexpected error occurred!"), context.RequestAborted);

        }
    }
}
