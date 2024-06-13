namespace CodeNet.Abstraction.Model;

public class MongoDBSettings
{
    public required string ConnectionString { get; set; }
    public required string DatabaseName { get; set; }
}
