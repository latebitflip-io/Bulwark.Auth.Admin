using Microsoft.AspNetCore.Diagnostics;

namespace Bulwark.Admin.Api.Controllers;

public class IndexController : ControllerBase
{
	public IndexController() 
	{

	}
    
    [HttpGet]
    [Route("health")]
    public ActionResult Health()
    {
        return StatusCode(200);
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("error")]
    public IActionResult HandleError() => Problem();

    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("/error-development")]
    public IActionResult HandleErrorDevelopment(
        [FromServices] IHostEnvironment hostEnvironment)
    {
        if (!hostEnvironment.IsDevelopment())
        {
            return NotFound();
        }

        var exceptionHandlerFeature =
            HttpContext.Features.Get<IExceptionHandlerFeature>()!;

        return Problem(
            detail: exceptionHandlerFeature.Error.StackTrace,
            title: exceptionHandlerFeature.Error.Message);
    }
}


