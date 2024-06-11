namespace NetCore.Abstraction.Model;

public abstract class BaseMongoDBModel : IBaseMongoDBModel
{
    /// <summary>
    /// Primary Key ID
    /// </summary>
    public Guid _id { get; set; }
}
