﻿namespace NetCore.Abstraction;

public interface ICacheRepository
{
    public object GetCache(string key);

    public void SetCache(string key, object value, int time);
}
