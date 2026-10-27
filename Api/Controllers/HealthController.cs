using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SampleDotNet6App.Api.Controllers;

[ApiController]
[Route("api/health")]
[AllowAnonymous]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get() => Ok(new { status = "Healthy" });

    [HttpGet("detailed")]
    public IActionResult Detailed() => Ok(new
    {
        status = "Healthy",
        timestampUtc = DateTime.UtcNow,
        version = "1.0.0"
    });
}
