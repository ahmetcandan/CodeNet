namespace CodeNet.MakerChecker.Models;

[AttributeUsage(AttributeTargets.Property)]
public class AutoIncrementAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Property)]
public class PrimaryKeyAttribute : Attribute
{
}
