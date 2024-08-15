using Microsoft.JSInterop;

namespace CodeNetUI_Example.Services;

public class LocalStorageManager(IJSRuntime jsRuntime)
{
    public ValueTask<string> SetAsync(string key, string value)
    {
        return SetAsync<string>(key, value);
    }

    public ValueTask<T> SetAsync<T>(string key, T value)
    {
        return jsRuntime.InvokeAsync<T>("localStorage.setItem", key, value);
    }

    public ValueTask<string> GetAsync(string key)
    {
        return GetAsync<string>(key);
    }

    public ValueTask<T> GetAsync<T>(string key)
    {
        return jsRuntime.InvokeAsync<T>("localStorage.getItem", key);
    }

    public ValueTask RemoveAsync(string key)
    {
        return jsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
    }
}
