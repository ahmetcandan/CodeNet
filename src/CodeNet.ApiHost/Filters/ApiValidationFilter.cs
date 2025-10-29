using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.Json;

namespace CodeNet.ApiHost.Filters;

internal class ApiValidationFilter : IEndpointFilter
{
    internal static readonly string[] _sourceArray = ["POST", "PUT"];
    internal static readonly JsonSerializerOptions _options = new() { WriteIndented = true, PropertyNameCaseInsensitive = true };

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        if (_sourceArray.Contains(context.HttpContext.Request.Method))
        {
            var requestType = context.HttpContext.GetEndpoint()?.Metadata.GetMetadata<MethodInfo>()?.GetParameters().FirstOrDefault()?.ParameterType;
            if (requestType is not null && requestType.GetProperties().SelectMany(p => p.GetCustomAttributes<ValidationAttribute>()).Any())
            {
                context.HttpContext.Request.EnableBuffering();
                if (context.HttpContext.Request.ContentLength > 0 &&
                    context.HttpContext.Request.ContentType?.StartsWith("application/json", StringComparison.OrdinalIgnoreCase) == true)
                {
                    using var reader = new StreamReader(context.HttpContext.Request.Body, leaveOpen: true);
                    var json = await reader.ReadToEndAsync();
                    reader.BaseStream.Position = 0;
                    var obj = JsonSerializer.Deserialize(json, requestType, _options);
                    if (obj is not null)
                    {
                        var validationContext = new ValidationContext(obj);
                        var results = new List<ValidationResult>();
                        if (!Validator.TryValidateObject(obj, validationContext, results, validateAllProperties: true))
                        {
                            var errors = results.ToDictionary(
                                r => r.MemberNames.FirstOrDefault() ?? "Error",
                                r => r.ErrorMessage != null ? [r.ErrorMessage] : Array.Empty<string>()
                            );
                            return Results.ValidationProblem(errors);
                        }
                    }
                }
            }
        }

        return await next(context);
    }
}
