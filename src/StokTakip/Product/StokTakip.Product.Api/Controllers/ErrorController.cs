using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NetCore.Abstraction;
using NetCore.Abstraction.Model;
using System.Reflection;

namespace StokTakip.Product.Api.Controllers;

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
    public IActionResult Index()
    {
        var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
        if (exceptionFeature is not null)
            _appLogger.ExceptionLog(exceptionFeature.Error, MethodBase.GetCurrentMethod()!);

        return BadRequest(new ResponseBase("99", "An unexpected error occurred!"));
    }
}
