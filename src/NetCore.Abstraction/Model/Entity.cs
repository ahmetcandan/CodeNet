namespace NetCore.Abstraction.Model;

public abstract class Entity : IEntity
{
    protected TValue GetValue<TValue>(TValue value)
    {
        return value;
    }

    protected TValue SetValue<TValue>(TValue value)
    {
        return value;
    }
}
