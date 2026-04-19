using Microsoft.AspNetCore.Mvc;
using Task5.Models;
using Task5.Services;

namespace Task5.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AudioController(AudioGeneratorService audioGeneratorService) : ControllerBase
{
    [HttpGet]
    public IActionResult Get([FromQuery] AudioParams parameters)
    {
        var bytes = audioGeneratorService.Generate(parameters.Seed, parameters.Index);
        Response.Headers.CacheControl = "public, max-age=3600";
        return File(bytes, "audio/wav", enableRangeProcessing: true);
    }
}
