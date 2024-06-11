namespace NetCore.Abstraction
{
    /// <summary>
    /// MongoDB CollectionName Attribute
    /// </summary>
    /// <param name="name"></param>
    [AttributeUsage(AttributeTargets.Class)]
    public class CollectionNameAttribute(string name) : Attribute
    {
        public string Name { get; set; } = name;
    }
}
