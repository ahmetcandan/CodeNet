namespace CodeNet.Abstraction
{
    /// <summary>
    /// Elasticsearch Index Name Attribute
    /// </summary>
    /// <param name="name"></param>
    [AttributeUsage(AttributeTargets.Class)]
    public class IndexNameAttribute(string name) : Attribute
    {
        public string Name { get; set; } = name;
    }
}
