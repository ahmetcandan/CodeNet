using System.Linq.Expressions;

namespace CodeNet.Mapper.Configurations;

public class MapperColumnBuilder<TSource, TDestination>
    where TSource : new()
    where TDestination : new()
{
    internal Dictionary<string, string> Columns { get; } = [];
    internal static MapType MapType => new(typeof(TSource), typeof(TDestination));

    internal bool AddMapColumn(Expression<Func<TSource, object>> sourceColumn, Expression<Func<TDestination, object>> destinationColumn)
        => Columns.TryAdd(string.Join('.', sourceColumn.Body.ToString().Split('.')[1..]), string.Join('.', destinationColumn.Body.ToString().Split('.')[1..]));
}
