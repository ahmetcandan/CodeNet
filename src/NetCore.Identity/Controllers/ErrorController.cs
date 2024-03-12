using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetCore.Abstraction;
using NetCore.Abstraction.Model;

namespace NetCore.Identity.Controllers;

[Route("Error")]
[ApiController]
public class ErrorController(IAppLogger appLogger) : ControllerBase
{
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
            appLogger.ExceptionLog(exceptionFeature.Error, typeof(ErrorController).GetMethod("Index"));

        return BadRequest(new ResponseBase
        {
            IsSuccessfull = false,
            MessageCode = "99",
            Message = "An unexpected error occurred!"
        });
    }
}
