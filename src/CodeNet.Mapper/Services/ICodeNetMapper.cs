namespace CodeNet.Mapper.Services;

public interface ICodeNetMapper
{
    TDestination? MapTo<TSource, TDestination>(TSource source)
        where TDestination : new()
        where TSource : new();
}
