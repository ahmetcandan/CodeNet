using System.Linq.Expressions;

namespace CodeNet.Mapper.Configurations;

public class MapperColumnBuilder<TSource, TDestination>
    where TSource : new()
    where TDestination : new()
{
    public MapperColumnBuilder()
    {
        MapperItem = new MapperItem(typeof(TSource), typeof(TDestination));
    }

    internal MapperItem MapperItem { get; }

    internal bool AddMapColumn(Expression<Func<TSource, object>> sourceColumn, Expression<Func<TDestination, object>> destinationColumn)
    {
        return MapperItem.AddColumn(string.Join('.', sourceColumn.Body.ToString().Split('.')[1..]), string.Join('.', destinationColumn.Body.ToString().Split('.')[1..]));
    }

    internal void SetMaxDepth(int maxDepth)
    {
        MapperItem.MaxDepth = maxDepth;
    }
}
