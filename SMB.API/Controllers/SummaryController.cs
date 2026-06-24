using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMB.API.Contracts;
using SMB.APPLICATION.DTOs.Summary;
using SMB.APPLICATION.Interfaces.Services;

namespace SMB.API.Controllers;

[ApiController]
[Route("summary")]
[Authorize]
public class SummaryController(ISummaryService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetSummary()
    {
        var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var summary = await service.GetSummaryByUserId(userId);

        var result = new Answer<DashboardSummaryDto>()
        {
            Message = "Se ha recuperado la información correctamente",
            Response = summary,
            Code = StatusCodes.Status200OK
        };

        return Ok(result);
    }
}