namespace NetCore.Abstraction;

public interface IElasticsearchModel
{
    Guid Id { get; }
    DateTime Date { get; }
}
