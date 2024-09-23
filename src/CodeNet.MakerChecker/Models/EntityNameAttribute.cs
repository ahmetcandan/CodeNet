namespace CodeNet.MakerChecker.Models;

[AttributeUsage(AttributeTargets.Class)]
public class EntityNameAttribute(string name) : Attribute
{
    public string Name { get; set; } = name;
}
