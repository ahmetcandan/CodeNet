using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using NetCore.Abstraction;
using NetCore.Abstraction.Model;
using NetCore.MongoDB;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace NetCore.Gateway
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly ILogRepository logRepository;
        private readonly RequestDelegate next;

        public RequestResponseLoggingMiddleware(RequestDelegate next, IOptions<MongoDbSettings> config)
        {
            this.next = next;
            logRepository = new MongoDBLogRepository(config);
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Request.EnableBuffering();

            var builder = new StringBuilder();

            var request = await FormatRequest(context.Request);

            builder.Append("Request: ").AppendLine(request);
            builder.AppendLine("Request headers:");
            foreach (var header in context.Request.Headers)
            {
                builder.Append(header.Key).Append(':').AppendLine(header.Value);
            }

            var originalBodyStream = context.Response.Body;

            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            await next(context);

            var response = await FormatResponse(context.Response);
            builder.Append("Response: ").AppendLine(response);
            builder.AppendLine("Response headers: ");
            foreach (var header in context.Response.Headers)
            {
                builder.Append(header.Key).Append(':').AppendLine(header.Value);
            }

            logRepository.Insert(new LogModel()
            {
                ClassName = context.Request.Path.ToString(),
                LogTime = Abstraction.Enum.LogTime.Before,
                Message = builder.ToString(),
                LogType = Abstraction.Enum.LogType.Info,
                MethodName = context.Request.Method,
                MethodParameters = new List<MethodParameter>()
                    {
                        new MethodParameter
                        {
                            Name = "queryString",
                            Value = context.Request.QueryString.ToString()
                        }
                    },
                Namespace = context.Request.Scheme,
                UserName = context.Request.HttpContext.User.Identity.Name
            });

            await responseBody.CopyToAsync(originalBodyStream);
        }

        private async Task<string> FormatRequest(HttpRequest request)
        {
            using var reader = new StreamReader(
                request.Body,
                encoding: Encoding.UTF8,
                detectEncodingFromByteOrderMarks: false,
                leaveOpen: true);
            var body = await reader.ReadToEndAsync();

            var formattedRequest = $"{request.Scheme} {request.Host}{request.Path} {request.QueryString} {body}";

            request.Body.Position = 0;

            return formattedRequest;
        }

        private async Task<string> FormatResponse(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);

            string text = await new StreamReader(response.Body).ReadToEndAsync();

            response.Body.Seek(0, SeekOrigin.Begin);

            return $"{response.StatusCode}: {text}";
        }
    }
}
