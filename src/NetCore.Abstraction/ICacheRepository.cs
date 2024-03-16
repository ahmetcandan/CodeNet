namespace NetCore.Abstraction;

public interface ICacheRepository
{
    public TModel GetCache<TModel>(string key);

    public void SetCache(string key, object value, int time);
}
