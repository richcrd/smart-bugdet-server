using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMB.API.Contracts;
using SMB.APPLICATION.DTOs.Analytics;
using SMB.APPLICATION.Interfaces.Services;

namespace SMB.API.Controllers;

[ApiController]
[Route("analytics")]
[Authorize]
public class AnalyticsController(IAnalyticsService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAnalytics()
    {
        var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var analytics = await service.GetAnalyticsByUserId(userId);

        var result = new Answer<AnalyticsResponseDto>
        {
            Message = "Analíticas recuperadas correctamente",
            Response = analytics,
            Code = StatusCodes.Status200OK
        };

        return Ok(result);
    }
}
