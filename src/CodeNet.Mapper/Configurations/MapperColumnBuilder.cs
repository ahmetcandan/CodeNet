using System.Linq.Expressions;

namespace CodeNet.Mapper.Configurations;

public class MapperColumnBuilder<TSource, TDestination>
    where TSource : new()
    where TDestination : new()
{
    private readonly MapperItem _item;

    public MapperColumnBuilder()
    {
        _item = new MapperItem(typeof(TSource), typeof(TDestination));
    }

    internal MapperItem MapperItem { get { return _item; } }

    internal bool AddMapColumn(Expression<Func<TSource, object>> sourceColumn, Expression<Func<TDestination, object>> destinationColumn)
    {
        return _item.AddColumn(string.Join('.', sourceColumn.Body.ToString().Split('.')[1..]), string.Join('.', destinationColumn.Body.ToString().Split('.')[1..]));
    }

    internal void SetMaxDepth(int maxDepth)
    {
        _item.MaxDepth = maxDepth;
    }
}
