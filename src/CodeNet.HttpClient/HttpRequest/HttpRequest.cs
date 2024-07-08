using CodeNet.HttpClient.Options;
using CodeNet.Logging;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using Http = System.Net.Http;

namespace CodeNet.HttpClient;

internal class HttpRequest(IHttpContextAccessor HttpContextAccessor, IAppLogger AppLogger) : IHttpRequest
{
    public async Task<TResponse?> GetAsync<TResponse>(string url, IDictionary<string, string>? headers = null, HttpClientOptions? httpClientOptions = default, CancellationToken cancellationToken = default)
    {
        return await SendAsync<TResponse>(HttpMethod.Get, url, headers: headers, httpClientOptions: httpClientOptions, cancellationToken: cancellationToken);
    }

    public async Task<TResponse?> PostAsync<TResponse>(string url, object data, IDictionary<string, string>? headers = null, HttpClientOptions? httpClientOptions = default, CancellationToken cancellationToken = default)
    {
        return await SendAsync<TResponse>(HttpMethod.Post, url, content: data, headers: headers, httpClientOptions: httpClientOptions, cancellationToken: cancellationToken);
    }

    public async Task<TResponse?> PutAsync<TResponse>(string url, object data, IDictionary<string, string>? headers = null, HttpClientOptions? httpClientOptions = default, CancellationToken cancellationToken = default)
    {
        return await SendAsync<TResponse>(HttpMethod.Put, url, content: data, headers: headers, httpClientOptions: httpClientOptions, cancellationToken: cancellationToken);
    }

    public async Task<TResponse?> DeleteAsync<TResponse>(string url, IDictionary<string, string>? headers = null, HttpClientOptions? httpClientOptions = default, CancellationToken cancellationToken = default)
    {
        return await SendAsync<TResponse>(HttpMethod.Delete, url, headers: headers, httpClientOptions: httpClientOptions, cancellationToken: cancellationToken);
    }

    public async Task<TResponse?> SendAsync<TResponse>(HttpMethod httpMethod, string url, object? content = null, IDictionary<string, string>? headers = null, HttpClientOptions? httpClientOptions = default, CancellationToken cancellationToken = default)
    {
        try
        {
            var methodInfo = typeof(HttpRequest).GetMethod("SendAsync");
            var requestJson = content is not null ? JsonConvert.SerializeObject(content) : string.Empty;
            AppLogger.EntryLog(new { Url = $"[{httpMethod.Method}]{url}", Request = requestJson }, methodInfo!);
            var timer = new Stopwatch();
            timer.Start();

            var httpClient = new Http.HttpClient();
            var httpRequest = new HttpRequestMessage(httpMethod, url);
            if (httpClientOptions?.UseCurrentHeaders is true && HttpContextAccessor.HttpContext is not null)
                foreach (var item in HttpContextAccessor.HttpContext.Request.Headers)
                    httpRequest.Headers.Add(item.Key, [.. item.Value]);

            if (headers is not null)
                foreach (var item in headers)
                {
                    if (httpRequest.Headers.Contains(item.Key))
                        httpRequest.Headers.Remove(item.Key);
                    httpRequest.Headers.Add(item.Key, item.Value);
                }

            if (content is not null)
                httpRequest.Content = new StringContent(requestJson, Encoding.UTF8, "application/json");

            var httpResponse = await httpClient.SendAsync(httpRequest, cancellationToken);

            if (httpClientOptions?.ExceptionHandling is false)
                httpResponse.EnsureSuccessStatusCode();

            var responseJson = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
            var response = JsonConvert.DeserializeObject<TResponse>(responseJson);
            timer.Stop();
            AppLogger.ExitLog(new { Url = $"[{httpMethod.Method}]{url}", Response = responseJson }, methodInfo!, timer.ElapsedMilliseconds);
            return response;
        }
        catch (Exception ex)
        {
            AppLogger.ExceptionLog(ex, MethodBase.GetCurrentMethod()!);

            if (httpClientOptions?.ExceptionHandling is false)
                throw;

            return default;
        }
    }
}
