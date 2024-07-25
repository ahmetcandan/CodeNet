using CodeNet.BackgroundJob.Models;
using CodeNet.EntityFramework.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CodeNet.BackgroundJob.Controllers;

//[ApiController]
//[Route("[controller]")]
//public class BackgrondServiceController(BackgroundJobDbContext dbContext) : ControllerBase
//{
//    [HttpGet("GetServices")]
//    [ProducesResponseType(200, Type = typeof(IEnumerable<Job>))]
//    public async Task<IActionResult> GetServices(int page, int count, CancellationToken cancellationToken)
//    {
//        var serviceRepository = new Repository<Job>(dbContext);
//        var result = await serviceRepository.GetPagingListAsync(page, count, cancellationToken);
//        return Ok(result);
//    }

//    [HttpGet("GetServiceDetails/{jobId}")]
//    [ProducesResponseType(200, Type = typeof(IEnumerable<Job>))]
//    public async Task<IActionResult> GetServiceDetails(int jobId, int page, int count, CancellationToken cancellationToken)
//    {
//        var jobWorkingDetailRepository = new Repository<JobWorkingDetail>(dbContext);
//        var result = await jobWorkingDetailRepository.GetPagingListAsync(c => c.JobId == jobId, page, count, cancellationToken);
//        return Ok(result);
//    }
//}
