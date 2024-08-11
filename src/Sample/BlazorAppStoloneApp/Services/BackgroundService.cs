using CodeNetUI_Example.Configurations;
using CodeNetUI_Example.Models;
using System.Net.Http.Json;
using System.Text;

namespace CodeNetUI_Example.Services;

public class BackgroundService(HttpClient http)
{
    public Task<PagingResponse<JobModel>?> GetJobs(int page, int count, CancellationToken cancellationToken = default)
    {
        return http.GetFromJsonAsync<PagingResponse<JobModel>>($"{AppSettings.BackgroundJobBaseUrl}/getServices?page={page}&count={count}", cancellationToken);
    }

    public Task<HttpResponseMessage> ExecuteJob(int jobId)
    {
        return http.PostAsJsonAsync<JobWorkingDetailModel>($"{AppSettings.BackgroundJobBaseUrl}/jobExecute?jobId={jobId}", null);
    }

    public Task<PagingResponse<JobWorkingDetailModel>?> GetJobDetails(int jobId, int page, int count, CancellationToken cancellationToken = default)
    {
        return http.GetFromJsonAsync<PagingResponse<JobWorkingDetailModel>>($"{AppSettings.BackgroundJobBaseUrl}/getServiceDetails?jobId={jobId}&page={page}&count={count}", cancellationToken);
    }

    public Task<HttpResponseMessage> DeleteDetails(int[] detailIds)
    {
        return http.SendAsync(new HttpRequestMessage(HttpMethod.Delete, $"{AppSettings.BackgroundJobBaseUrl}/deleteDetails")
        {
            Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(detailIds), Encoding.UTF8, System.Net.Mime.MediaTypeNames.Application.Json)
        });
    }
}
