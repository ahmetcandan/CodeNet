using CodeNet.MakerChecker.Models;

namespace CodeNet.MakerChecker.Extensions;

internal class MakerCheckerItem(Type entityType, string entityName)
{
    public string EntityName { get; set; } = entityName;
    public Type EntityType { get; set; } = entityType;
}

public class MakerCheckerBuilder
{
    private readonly List<MakerCheckerItem> _makerCheckerItems = [];

    internal List<MakerCheckerItem> MakerCheckerItems { get { return _makerCheckerItems; } }

    internal void AddItem(Type entityType, string entityName)
    {
        _makerCheckerItems.Add(new MakerCheckerItem(entityType, entityName));
    }
}

public static class MakerCheckerBuilderExtensions
{
    public static MakerCheckerBuilder AddEntity<TMakerCheckerEntity>(this MakerCheckerBuilder makerCheckerBuilder, string entityName)
        where TMakerCheckerEntity : class, IMakerCheckerEntity
    {
        makerCheckerBuilder.AddItem(typeof(TMakerCheckerEntity), entityName);
        return makerCheckerBuilder;
    }
}
