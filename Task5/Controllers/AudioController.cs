using Microsoft.AspNetCore.Mvc;
using Task5.Services;

namespace Task5.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AudioController(AudioGeneratorService audioGeneratorService) : ControllerBase
{
    [HttpGet]
    public IActionResult Get([FromQuery] long seed, [FromQuery] int index)
    {
        var bytes = audioGeneratorService.Generate(seed, index);
        Response.Headers.CacheControl = "public, max-age=3600";
        return File(bytes, "audio/wav", enableRangeProcessing: true);
    }
}
