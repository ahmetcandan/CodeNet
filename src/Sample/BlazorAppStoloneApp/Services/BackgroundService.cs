using CodeNetUI_Example.Configurations;
using CodeNetUI_Example.Models;

namespace CodeNetUI_Example.Services;

public class BackgroundService(ApiClientService apiClientService)
{
    public Task<PagingResponse<JobModel>?> GetJobs(int page, int count, CancellationToken cancellationToken = default)
    {
        return apiClientService.SendService<PagingResponse<JobModel>>(HttpMethod.Get, $"{AppSettings.BackgroundJobBaseUrl}/getServices?page={page}&count={count}", null, cancellationToken);
    }

    public Task<JobWorkingDetailModel?> ExecuteJob(int jobId)
    {
        return apiClientService.SendService<JobWorkingDetailModel>(HttpMethod.Post, $"{AppSettings.BackgroundJobBaseUrl}/jobExecute?jobId={jobId}", null, cancellationToken: CancellationToken.None);
    }

    public Task<PagingResponse<JobWorkingDetailModel>?> GetJobDetails(int jobId, int page, int count, CancellationToken cancellationToken = default)
    {
        return apiClientService.SendService<PagingResponse<JobWorkingDetailModel>>(HttpMethod.Get, $"{AppSettings.BackgroundJobBaseUrl}/getServiceDetails?jobId={jobId}&page={page}&count={count}", null, cancellationToken);
    }

    public Task<HttpResponseMessage?> DeleteDetails(int[] detailIds)
    {
        return apiClientService.SendService(HttpMethod.Delete, $"{AppSettings.BackgroundJobBaseUrl}/deleteDetails", detailIds, CancellationToken.None);
    }
}
