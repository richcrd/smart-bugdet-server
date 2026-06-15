using Microsoft.AspNetCore.Mvc;
using SMB.API.Contracts;
using SMB.APPLICATION.DTOs.Catalog;
using SMB.APPLICATION.Interfaces.Services;

namespace SMB.API.Controllers;

[ApiController]
[Route("catalog")]
public class CatalogController(ICatalogService catalogService) : ControllerBase
{
    [HttpGet("language")]
    public async Task<IActionResult> GetAllLanguages()
    {
        var languages = await catalogService.GetAllLanguages();

        var response = new Answer<List<LanguageResponse>>()
        {
            Message = "Se ha recuperado la información correctamente",
            Response = languages,
            Code = StatusCodes.Status200OK,
        };

        return Ok(response);
    }
}