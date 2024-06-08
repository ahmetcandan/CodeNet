using Autofac;
using MediatR;

namespace NetCore.ExceptionHandling.Module;

/// <summary>
/// Exception Handling Module
/// </summary>
public class ExceptionHandlingModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterGeneric(typeof(ExceptionHandler<,>)).As(typeof(IPipelineBehavior<,>));
        base.Load(builder);
    }
}