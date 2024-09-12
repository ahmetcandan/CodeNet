using CodeNet.Mapper.Configurations;
using System.Linq.Expressions;

namespace CodeNet.Mapper.Extensions;

public static class MapperConfigurationBuilderExtensions
{
    internal const int DEFAULT_MAX_DEPTH = 4;

    public static MapperColumnBuilder<TSource, TDestination> Map<TSource, TDestination>(this MapperColumnBuilder<TSource, TDestination> builder, Expression<Func<TSource, object>> sourceSelector, Expression<Func<TDestination, object>> destinationSelector)
        where TSource : new()
        where TDestination : new()
    {
        builder.AddMapColumn(sourceSelector, destinationSelector);
        return builder;
    }
    public static MapperColumnBuilder<TSource, TDestination> MaxDepth<TSource, TDestination>(this MapperColumnBuilder<TSource, TDestination> builder, int maxDepth)
    where TSource : new()
    where TDestination : new()
    {
        builder.SetMaxDepth(maxDepth);
        return builder;
    }
}
