using CodeNet.MakerChecker.Models;

namespace CodeNet.MakerChecker.Extensions;

internal class MakerCheckerItem(Type entityType, string entityName)
{
    public string EntityName { get; set; } = entityName;
    public Type EntityType { get; set; } = entityType;
}

public class MakerCheckerBuilder
{
    internal List<MakerCheckerItem> MakerCheckerItems { get; } = [];

    internal void AddItem(Type entityType, string entityName) => MakerCheckerItems.Add(new MakerCheckerItem(entityType, entityName));
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
