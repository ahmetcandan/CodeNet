namespace NetCore.Core;

public abstract class MySwaggerClientBase
{
    public string BearerToken { get; private set; }

    public void SetBearerToken(string token)
    {
        BearerToken = token;
    }

    protected HttpRequestMessage CreateHttpRequestMessageAsync()
    {
        var msg = new HttpRequestMessage();
        msg.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", BearerToken);
        return msg;
    }

}
