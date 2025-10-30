using CodeNet.ApiHost.Attributes;
using CodeNet.ApiHost.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CodeNet.ApiHost.Extensions;

public static class ApiServiceMapperExtensions
{
    public static IServiceCollection AddApiHost(this IServiceCollection services, Type type)
    {
        return services.AddApiHost(type.Assembly);
    }

    public static IServiceCollection AddApiHost(this IServiceCollection services, Assembly assembly)
    {
        assembly ??= Assembly.GetExecutingAssembly();
        return services.AddApiHost(assembly.GetTypes());
    }

    private static IServiceCollection AddApiHost(this IServiceCollection services, Type[] types)
    {
        foreach (var type in types.Where(t => t.IsClass && !t.IsAbstract && typeof(IApiService).IsAssignableFrom(t)))
        {
            var implementationType = type.GetInterfaces().Where(c => c != typeof(IApiService)).FirstOrDefault();
            if (implementationType is null)
                services.AddScoped(type);
            else
                services.AddScoped(implementationType, type);
        }

        services.AddTransient<ApiValidationFilter>();
        services.AddTransient<ApiAuthorizationFilter>();

        return services;
    }

    public static WebApplication UseApiHost(this WebApplication app, string apiPrefix = "/api", Assembly? assembly = null)
    {
        assembly ??= Assembly.GetExecutingAssembly();
        var serviceTypes = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && typeof(IApiService).IsAssignableFrom(t));

        foreach (var serviceType in serviceTypes.Where(c => c is not null))
        {
            var apiRouteAttr = serviceType!.GetCustomAttribute<XApiRouteAttribute>();
            var classRoute = apiRouteAttr?.Route ?? serviceType!.Name.Replace("Service", "");
            var implementationType = serviceType!.GetInterfaces().Where(c => c != typeof(IApiService)).FirstOrDefault();

            var methods = serviceType!.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(m => m.GetCustomAttribute<XHttpMethodAttribute>() != null);

            if (implementationType is not null && implementationType != serviceType!)
            {
                var _methods = implementationType.GetMethods(BindingFlags.Public | BindingFlags.Instance);
                methods = from m in methods
                          join _m in _methods on m.Name equals _m.Name
                          select m;
            }

            foreach (var method in methods)
            {
                var methodAttr = method.GetCustomAttribute<XHttpMethodAttribute>()!;
                var requestDelegate = RequestDelegateFactory.Create(method,
                    (context) => context.RequestServices.GetRequiredService(implementationType ?? serviceType!))
                    .RequestDelegate;
                var endpointBuilder = app.MapMethods($"{apiPrefix}/{classRoute}/{methodAttr.Route ?? method.Name}".ToLowerInvariant().Replace("//", "/"),
                    [methodAttr.HttpMethod.ToUpperInvariant()],
                    requestDelegate);

                var validationFilter = app.Services.GetRequiredService<ApiValidationFilter>();
                var authFilter = app.Services.GetRequiredService<ApiAuthorizationFilter>();

                endpointBuilder.AddEndpointFilter(validationFilter);
                endpointBuilder.AddEndpointFilter(authFilter);

                var methodAuthAttr = method.GetCustomAttribute<XAuthAttribute>();
                var classAuthAttr = serviceType!.GetCustomAttribute<XAuthAttribute>();
                if (methodAuthAttr is not null || classAuthAttr is not null)
                    endpointBuilder.AddAuthorization(methodAuthAttr, classAuthAttr);

                if (method.GetParameters().Any(p => p.ParameterType.IsClass))
                    endpointBuilder.WithMetadata(new AcceptsMetadata(isOptional: false, contentTypes: ["application/json"]));
                endpointBuilder.WithMetadata(method);
            }
        }
        return app;
    }

    private static void AddAuthorization(this IEndpointConventionBuilder endpointBuilder, XAuthAttribute? methodAuthAttr, XAuthAttribute? classAuthAttr)
    {
        if (string.IsNullOrEmpty(methodAuthAttr?.Users) && string.IsNullOrEmpty(methodAuthAttr?.Roles) && string.IsNullOrEmpty(classAuthAttr?.Users) && string.IsNullOrEmpty(classAuthAttr?.Roles))
            endpointBuilder.RequireAuthorization();
        else
            endpointBuilder.RequireAuthorization(configure =>
            {
                if (!string.IsNullOrEmpty(methodAuthAttr?.Users))
                    foreach (var userName in methodAuthAttr.Users.SemicolonSplit())
                        configure.RequireUserName(userName);
                if (!string.IsNullOrEmpty(classAuthAttr?.Users))
                    foreach (var userName in classAuthAttr.Users.SemicolonSplit())
                        configure.RequireUserName(userName);

                if (!string.IsNullOrEmpty(methodAuthAttr?.Roles))
                    foreach (var role in methodAuthAttr.Roles.SemicolonSplit())
                        configure.RequireRole(role);
                if (!string.IsNullOrEmpty(classAuthAttr?.Roles))
                    foreach (var role in classAuthAttr.Roles.SemicolonSplit())
                        configure.RequireRole(role);
            });
    }
}