using CodeNet.Core.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Reflection;
using System.Text;

namespace CodeNet.Core;

public abstract class BaseMiddleware
{
    protected static MethodInfo? GetMethodInfo(HttpContext context) => context.GetEndpoint()?.Metadata.GetMetadata<ControllerActionDescriptor>()?.MethodInfo;

    protected static async Task<byte[]> StreamToByteArray(Stream stream, string controllerName, CancellationToken cancellationToken)
    {
        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream, cancellationToken);
        var arr1 = memoryStream.ToArray();
        var arr2 = Encoding.UTF8.GetBytes(controllerName);
        var result = new byte[arr1.Length + arr2.Length];
        Buffer.BlockCopy(arr1, 0, result, 0, arr1.Length);
        Buffer.BlockCopy(arr2, 0, result, arr1.Length, arr2.Length);
        return result;
    }
    protected static async Task<byte[]> StreamToByteArray(Stream stream, CancellationToken cancellationToken)
    {
        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream, cancellationToken);
        return memoryStream.ToArray();
    }

    protected static async Task<string> ReadResponseAsync(HttpContext context, RequestDelegate next)
    {
        using var swapStream = new MemoryStream();
        var originalResponseBody = context.Response.Body;
        context.Response.Body = swapStream;
        await next(context);
        swapStream.Seek(0, SeekOrigin.Begin);
        string responseBody = await (new StreamReader(swapStream)).ReadToEndAsync(context.RequestAborted);
        swapStream.Seek(0, SeekOrigin.Begin);
        await swapStream.CopyToAsync(originalResponseBody, context.RequestAborted);
        context.Response.Body = originalResponseBody;
        return responseBody;
    }

    protected static async Task<string> GetRequestKey(HttpContext context, MethodInfo methodInfo)
    {
        if (context.Request.ContentType is null)
            return Hashing.MD5((context.Request.Body, methodInfo.DeclaringType?.FullName ?? string.Empty) + string.Join("", context.Request.RouteValues.Select(c => c.Value)));
        else
            return Hashing.MD5(await StreamToByteArray(context.Request.Body, methodInfo.DeclaringType?.FullName ?? string.Empty, context.RequestAborted));
    }

    protected static async Task<string> GetRequest(HttpContext context)
    {
        if (context.Request.ContentType is null)
            return string.Join("", context.Request.RouteValues.Select(c => c.Value));
        else
            return Encoding.UTF8.GetString(await StreamToByteArray(context.Request.Body, context.RequestAborted));
    }
}
