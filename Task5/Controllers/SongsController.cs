using Microsoft.AspNetCore.Mvc;
using Task5.Models;
using Task5.Services;

namespace Task5.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SongsController(DataGeneratorService dataGeneratorService, LocaleDataService localeDataService) : ControllerBase
{
    [HttpGet]
    public IActionResult Get([FromQuery] GenerationParams parameters)
    {
        parameters.Locale = LocaleResolver.Resolve(parameters.Locale, Request.Headers.AcceptLanguage.ToString(), localeDataService);
        var songs = dataGeneratorService.GeneratePage(parameters);
        return Ok(songs);
    }
}
