using Microsoft.AspNetCore.Mvc;
using Task5.Services;

namespace Task5.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LocalesController(LocaleDataService localeDataService) : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(localeDataService.GetAvailableLocales());
    }
}
