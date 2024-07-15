using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CodeNet.ExceptionHandling;

internal sealed class ExceptionHandlerMiddleware(RequestDelegate next)
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
            string? errorCode = null, errorMessage = null, title = null;
            bool defaultMessage = false;
            switch (ex)
            {
                case UserLevelException userLevelException:
                    errorCode = userLevelException.Code;
                    errorMessage = userLevelException.Message;
                    context.Response.StatusCode = userLevelException?.HttpStatusCode ?? StatusCodes.Status500InternalServerError;
                    title = nameof(UserLevelException);
                    break;
                case CodeNetException codeNetException:
                    context.Response.StatusCode = codeNetException?.HttpStatusCode ?? StatusCodes.Status500InternalServerError;
                    title = nameof(CodeNetException);
                    break;
                case BadHttpRequestException badHttpRequestException:
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    title = nameof(BadHttpRequestException);
                    break;
                default:
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    defaultMessage = true;
                    break;
            }

            if (defaultMessage)
            {
                var defaultProblemDetails = context.RequestServices.GetRequiredService<IOptions<ProblemDetails>>();
                if (!string.IsNullOrEmpty(defaultProblemDetails?.Value?.Detail))
                {
                    await context.Response.WriteAsJsonAsync(defaultProblemDetails.Value, context.RequestAborted);
                    return;
                }
            }

            await context.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Detail = $"{errorCode ?? "EX0001"} - {errorMessage ?? "An unexpected error occurred!"}",
                Title = title ?? "InternalServerError",
                Status = context.Response.StatusCode,
                Instance = context.Request.Path
            }, context.RequestAborted);
        }
    }
}
