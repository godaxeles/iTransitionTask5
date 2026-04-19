using Microsoft.AspNetCore.Mvc;
using Task5.Models;
using Task5.Services;

namespace Task5.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExportController(SongPackager songPackager, LocaleDataService localeDataService) : ControllerBase
{
    private const int MaxExportCount = 50;

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GenerationParams parameters, CancellationToken cancellationToken)
    {
        parameters.Locale = LocaleResolver.Resolve(parameters.Locale, Request.Headers.AcceptLanguage.ToString(), localeDataService);
        parameters.PageSize = Math.Clamp(parameters.PageSize, 1, MaxExportCount);
        parameters.Page = 1;

        var bytes = await songPackager.CreateZipAsync(parameters, cancellationToken);
        return File(bytes, "application/zip", BuildFileName(parameters));
    }

    private static string BuildFileName(GenerationParams parameters)
        => $"musicstore-{parameters.Locale}-{parameters.Seed}.zip";
}
