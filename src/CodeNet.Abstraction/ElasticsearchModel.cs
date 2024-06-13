namespace CodeNet.Abstraction;

public class ElasticsearchModel : IElasticsearchModel
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime Date { get; } = DateTime.Now;
}
