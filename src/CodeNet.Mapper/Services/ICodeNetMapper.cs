using System.Diagnostics.CodeAnalysis;

namespace CodeNet.Mapper.Services;

public interface ICodeNetMapper
{
    TDestination MapTo<TSource, TDestination>([NotNull] TSource source)
        where TDestination : new()
        where TSource : new();
}
