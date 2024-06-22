using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using CodeNet.Core.Models;
using CodeNet.Logging;

namespace CodeNet.ExceptionHandling.Controllers;


[Route("Error")]
[ApiController]
public class ErrorController(IAppLogger appLogger) : ControllerBase
{
    private readonly IAppLogger _appLogger = appLogger;

    [HttpDelete]
    [HttpGet]
    [HttpHead]
    [HttpOptions]
    [HttpPatch]
    [HttpPost]
    [HttpPut]
    public virtual IActionResult Index()
    {
        var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
        if (exceptionFeature is not null)
            _appLogger.ExceptionLog(exceptionFeature.Error, MethodBase.GetCurrentMethod()!);

        return BadRequest(new ResponseBase("99", "An unexpected error occurred!"));
    }
}
