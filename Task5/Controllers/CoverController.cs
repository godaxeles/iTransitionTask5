using Microsoft.AspNetCore.Mvc;
using Task5.Models;
using Task5.Services;

namespace Task5.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoverController(CoverGeneratorService coverGeneratorService) : ControllerBase
{
    [HttpGet]
    public IActionResult Get([FromQuery] CoverParams parameters)
    {
        var bytes = coverGeneratorService.Generate(
            parameters.Seed,
            parameters.Index,
            parameters.Title,
            parameters.Artist);

        Response.Headers.CacheControl = "public, max-age=3600";
        return File(bytes, "image/png");
    }
}
