using CodeNet.ApiHost.Attributes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;
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
        foreach (var serviceType in types.Where(t => t.IsClass && !t.IsAbstract && typeof(IApiService).IsAssignableFrom(t)))
        {
            var serviceInterface = serviceType.GetInterfaces().FirstOrDefault(c => c != typeof(IApiService));
            if (serviceInterface is null)
                services.AddScoped(serviceType);
            else
                services.AddScoped(serviceInterface, serviceType);
        }

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
            var implementationType = serviceType!.GetInterfaces().FirstOrDefault(c => c != typeof(IApiService));

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
                    (context) =>
                    {
                        return context.RequestServices.GetRequiredService(implementationType ?? serviceType!);
                    })
                    .RequestDelegate;
                var endpointBuilder = app.MapMethods($"{apiPrefix}/{classRoute}/{methodAttr.Route ?? method.Name}".ToLowerInvariant().Replace("//", "/"),
                    [methodAttr.HttpMethod.ToUpperInvariant()],
                    requestDelegate);

                var methodAuthAttr = method.GetCustomAttribute<XAuthorizeAttribute>();
                var classAuthAttr = serviceType!.GetCustomAttribute<XAuthorizeAttribute>();
                if (methodAuthAttr is not null || classAuthAttr is not null)
                    endpointBuilder.AddAuthorization(methodAuthAttr, classAuthAttr);

                if (method.GetParameters().Any(p => p.ParameterType.IsClass))
                    endpointBuilder.WithMetadata(method, new AcceptsMetadata(isOptional: true, type: method.GetParameters().First(c => c.ParameterType.IsClass).ParameterType, contentTypes: ["application/json"]));
                else
                    endpointBuilder.WithMetadata(method);
            }
        }
        return app;
    }

    private static void AddAuthorization(this IEndpointConventionBuilder endpointBuilder, XAuthorizeAttribute? methodAuthAttr, XAuthorizeAttribute? classAuthAttr)
    {
        if (string.IsNullOrEmpty(methodAuthAttr?.Users) && string.IsNullOrEmpty(methodAuthAttr?.Roles) && string.IsNullOrEmpty(classAuthAttr?.Users) && string.IsNullOrEmpty(classAuthAttr?.Roles))
            endpointBuilder.RequireAuthorization();
        else
            endpointBuilder.RequireAuthorization(configure =>
            {
                ApplyUserRequirements(methodAuthAttr, classAuthAttr, configure);
                ApplyRoleRequirements(methodAuthAttr, classAuthAttr, configure);
            });
    }

    private static void ApplyRoleRequirements(XAuthorizeAttribute? methodAuthAttr, XAuthorizeAttribute? classAuthAttr, Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder configure)
    {
        if (!string.IsNullOrEmpty(methodAuthAttr?.Roles))
            foreach (var role in methodAuthAttr.Roles.SemicolonSplit())
                configure.RequireRole(role);
        if (!string.IsNullOrEmpty(classAuthAttr?.Roles))
            foreach (var role in classAuthAttr.Roles.SemicolonSplit())
                configure.RequireRole(role);
    }

    private static void ApplyUserRequirements(XAuthorizeAttribute? methodAuthAttr, XAuthorizeAttribute? classAuthAttr, Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder configure)
    {
        if (!string.IsNullOrEmpty(methodAuthAttr?.Users))
            foreach (var userName in methodAuthAttr.Users.SemicolonSplit())
                configure.RequireUserName(userName);
        if (!string.IsNullOrEmpty(classAuthAttr?.Users))
            foreach (var userName in classAuthAttr.Users.SemicolonSplit())
                configure.RequireUserName(userName);
    }
}
