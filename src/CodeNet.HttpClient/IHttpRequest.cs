using CodeNet.HttpClient.Options;

namespace CodeNet.HttpClient;

public interface IHttpRequest
{
    Task<TResponse?> GetAsync<TResponse>(string url, IDictionary<string, string>? headers = null, HttpClientOptions? httpClientOptions = default, CancellationToken cancellationToken = default);
    Task<TResponse?> PutAsync<TResponse>(string url, object data, IDictionary<string, string>? headers = null, HttpClientOptions? httpClientOptions = default, CancellationToken cancellationToken = default);
    Task<TResponse?> PostAsync<TResponse>(string url, object data, IDictionary<string, string>? headers = null, HttpClientOptions? httpClientOptions = default, CancellationToken cancellationToken = default);
    Task<TResponse?> DeleteAsync<TResponse>(string url, IDictionary<string, string>? headers = null, HttpClientOptions? httpClientOptions = default, CancellationToken cancellationToken = default);
    Task<TResponse?> SendAsync<TResponse>(HttpMethod httpMethod, string url, object? content = null, IDictionary<string, string>? headers = null, HttpClientOptions? httpClientOptions = default, CancellationToken cancellationToken = default);
}
