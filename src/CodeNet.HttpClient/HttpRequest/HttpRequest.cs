using CodeNet.Core;
using CodeNet.Core.Settings;
using CodeNet.HttpClient.Options;
using CodeNet.Logging;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using Http = System.Net.Http;

namespace CodeNet.HttpClient;

internal class HttpRequest(ICodeNetContext codeNetContext, IAppLogger appLogger) : IHttpRequest
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
            appLogger.EntryLog(new { Url = $"[{httpMethod.Method}]{url}", Request = requestJson }, methodInfo!);
            var timer = new Stopwatch();
            timer.Start();

            var httpClient = new Http.HttpClient();
            var httpRequest = new HttpRequestMessage(httpMethod, url);

            if (httpClientOptions?.UseCurrentHeaders is null or true)
            {
                if (codeNetContext?.RequestHeaders?.ContainsKey(HeaderNames.Authorization) is true)
                    httpRequest.Headers.Add(HeaderNames.Authorization, [.. codeNetContext.RequestHeaders[HeaderNames.Authorization]]);

                if (codeNetContext?.RequestHeaders?.ContainsKey(Constant.CorrelationId) is true)
                    httpRequest.Headers.Add(Constant.CorrelationId, [.. codeNetContext.RequestHeaders[Constant.CorrelationId]]);

                if (codeNetContext?.RequestHeaders?.ContainsKey(HeaderNames.UserAgent) is true)
                    httpRequest.Headers.Add(HeaderNames.UserAgent, [.. codeNetContext.RequestHeaders[HeaderNames.UserAgent]]);

                if (codeNetContext?.RequestHeaders?.ContainsKey(HeaderNames.AcceptLanguage) is true)
                    httpRequest.Headers.Add(HeaderNames.AcceptLanguage, [.. codeNetContext.RequestHeaders[HeaderNames.AcceptLanguage]]);
            }

            if (headers is not null)
                foreach (var item in headers)
                {
                    if (httpRequest.Headers.Contains(item.Key))
                        httpRequest.Headers.Remove(item.Key);
                    httpRequest.Headers.Add(item.Key, item.Value);
                }
            
            if (content is not null)
                httpRequest.Content = new StringContent(requestJson, Encoding.UTF8, System.Net.Mime.MediaTypeNames.Application.Json);

            var httpResponse = await httpClient.SendAsync(httpRequest, cancellationToken);

            if (httpClientOptions?.ExceptionHandling is null or false)
                httpResponse.EnsureSuccessStatusCode();

            var responseJson = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
            var response = JsonConvert.DeserializeObject<TResponse>(responseJson);
            timer.Stop();
            appLogger.ExitLog(new { Url = $"[{httpMethod.Method}]{url}", Response = responseJson }, methodInfo!, timer.ElapsedMilliseconds);
            return response;
        }
        catch (Exception ex)
        {
            appLogger.ExceptionLog(ex, MethodBase.GetCurrentMethod()!);

            if (httpClientOptions?.ExceptionHandling is null or false)
                throw;

            return default;
        }
    }
}
