using CodeNet.Core;
using CodeNet.Core.Settings;
using CodeNet.HttpClient.Options;
using CodeNet.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using Http = System.Net.Http;

namespace CodeNet.HttpClient;

internal class HttpRequest(IServiceProvider serviceProvider) : IHttpRequest
{
    private readonly ICodeNetContext? _codeNetContext = serviceProvider.GetService<ICodeNetContext>();
    private readonly IAppLogger? _appLogger = serviceProvider.GetService<IAppLogger>();

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
            _appLogger?.EntryLog(new { Url = $"[{httpMethod.Method}]{url}", Request = requestJson }, methodInfo!);
            var timer = new Stopwatch();
            timer.Start();

            var httpClient = new Http.HttpClient();
            var httpRequest = new HttpRequestMessage(httpMethod, url);

            if (httpClientOptions?.UseCurrentHeaders is null or true)
            {
                if (_codeNetContext?.RequestHeaders?.ContainsKey(HeaderNames.Authorization) is true)
                    httpRequest.Headers.Add(HeaderNames.Authorization, [.. _codeNetContext.RequestHeaders[HeaderNames.Authorization]]);

                if (_codeNetContext?.RequestHeaders?.ContainsKey(Constant.CorrelationId) is true)
                    httpRequest.Headers.Add(Constant.CorrelationId, [.. _codeNetContext.RequestHeaders[Constant.CorrelationId]]);

                if (_codeNetContext?.RequestHeaders?.ContainsKey(HeaderNames.UserAgent) is true)
                    httpRequest.Headers.Add(HeaderNames.UserAgent, [.. _codeNetContext.RequestHeaders[HeaderNames.UserAgent]]);

                if (_codeNetContext?.RequestHeaders?.ContainsKey(HeaderNames.AcceptLanguage) is true)
                    httpRequest.Headers.Add(HeaderNames.AcceptLanguage, [.. _codeNetContext.RequestHeaders[HeaderNames.AcceptLanguage]]);
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
            _appLogger?.ExitLog(new { Url = $"[{httpMethod.Method}]{url}", Response = responseJson }, methodInfo!, timer.ElapsedMilliseconds);
            return response;
        }
        catch (Exception ex)
        {
            _appLogger?.ExceptionLog(ex, MethodBase.GetCurrentMethod()!);

            if (httpClientOptions?.ExceptionHandling is null or false)
                throw;

            return default;
        }
    }
}
